using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using DG.Tweening;

namespace UnityEngine.AI.MonsterBehavior
{
    public class EnemyBehaviours : MonoBehaviour
    {
        [SerializeField] private float enemyHealth = 100f;
        [SerializeField] private float movementSpeed = 1.5f;
        [SerializeField] private float SprintSpeed = 3.5f;
        [SerializeField] private float stunnedTimeAfterFamageTaken = 0.5f;
        [SerializeField] private float maxDistanceToTarget = 10f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackDuration = 0.6f;
        [SerializeField] private float minHitDamage = 5f;
        [SerializeField] private float maxHitDamage = 25f;

        [SerializeField] private string[] attackAnimatonsList;

        [SerializeField] private ParticleSystem counterParticles;

        public EnemyStateType enemyStateTypeDropdown;
        public enum EnemyStateType { Purposeless, Guardian };

        public LayerMask groundMask;

        public UnityEvent<PlayerHealth> HitThePlayer;

        private float distanceToTarget;
        private float currentSpeed;
        private float animatorSpeed;

        private int attackAnimationIndex;

        private bool[] enemyStateTypeBool = new bool[2];
        private bool isPraperingAttack;
        private bool isMoving;
        private bool isRetreating;
        private bool isLockedTarget;
        private bool isStunned;
        private bool isCharging;
        private bool isDead;
        private bool isPlayerInAttackRange;
        private bool isPlayingAttackAnimation;
        private bool leftClicked;

        //Purposeless Enemy
        [SerializeField] private float walkRange = 5;
        [SerializeField] private float waitTime = 3;

        private float currentWaitedTime;

        private bool destinationPointSet;

        //Guardian Enemy
        [SerializeField] private GameObject protectedResource;
        [SerializeField] private float resourceAreaBorderRange;
        [SerializeField] private float protectedAreaBorderRange;

        private GameObject player;
        private GameObject enemyTarget;

        private Coroutine prepareToAttackCoroutine;
        private Coroutine RetreatCoroutine;
        private Coroutine DamageCoroutine;
        private Coroutine MovementCoroutine;
        private Coroutine WaitForNothingCoroutine;

        private EnemyDetector enemyDetector;
        private PlayerCombat playerCombat;
        private CharacterMovement characterMovement;
        private WeaponController weaponController;
        private Animator animator;
        private NavMeshAgent agent;

        private Vector3 targetVector;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            enemyDetector = player.GetComponentInChildren<EnemyDetector>();
            characterMovement = player.GetComponent<CharacterMovement>();
            playerCombat = player.GetComponent<PlayerCombat>();
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();

            playerCombat.OnDamageTaken.AddListener((x) => OnPlayerHit(x));
            playerCombat.OnCounterAttack.AddListener((x) => OnPlayerCounter(x));
            playerCombat.OnLockedToEnemy.AddListener((x) => OnPlayerLockedToEnemy(x));

            enemyTarget = player;

            for (int i = 0; i < enemyStateTypeBool.Length; i++)
            {
                enemyStateTypeBool[i] = false;
            }

            enemyStateTypeBool[(int)enemyStateTypeDropdown] = true;
        }

        void Update()
        {
            agent.SetDestination(targetVector);
            agent.speed = currentSpeed;
            animator.SetFloat("Speed", animatorSpeed);

            if (!isDead)
            {
                //transform.LookAt(targetVector);
                var rotation = Quaternion.LookRotation(targetVector - transform.position, Vector3.up);
                rotation.y = 0;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 50 * Time.deltaTime);
                EnemyMovement();
            }
            else
            {
                animator.SetBool("Dead", true);
            }
            if (enemyHealth <= 0)
            {
                Death();
                animator.SetBool("Dead", true);
            }
        }

        void EnemyMovement()
        {
            distanceToTarget = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToTarget <= maxDistanceToTarget && distanceToTarget > attackRange)
            {
                targetVector = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

                DOVirtual.Float(currentSpeed, SprintSpeed, 0.4f, (speed) => currentSpeed = speed);
                DOVirtual.Float(animatorSpeed, 1, 0.1f, (speed) => animatorSpeed = speed);

                isPlayerInAttackRange = false;
            }
            else if(distanceToTarget < attackRange)
            {
                DOVirtual.Float(animatorSpeed, 0, 0.1f, (speed) => animatorSpeed = speed);
                isPlayerInAttackRange = true;
                EnemyAttack();
            }
            else
            {
                isPlayerInAttackRange = false;
                DOVirtual.Float(animatorSpeed, 0.5f, 0.1f, (speed) => animatorSpeed = speed);
                DOVirtual.Float(currentSpeed, movementSpeed, 0.4f, (speed) => currentSpeed = speed);

                switch ((int)enemyStateTypeDropdown)
                {
                    case 0:
                        PurposelessEnemyPatrol();
                        break;
                    case 1:
                        GuardianEnemyPatrol();
                        break;
                }
            }

        }

        void EnemyAttack()
        {
            StopMoving();
            if (RetreatCoroutine != null)
            {
                return;
            }
            RetreatCoroutine = StartCoroutine(Charging(attackDuration));

            IEnumerator Charging(float chargeDuration)
            {
                isCharging = true;

                yield return new WaitForSeconds(chargeDuration);
                
                RetreatCoroutine = null;
                animator.SetTrigger(attackAnimatonsList[attackAnimationIndex]);
                attackAnimationIndex = (int)Mathf.Repeat(attackAnimationIndex + 1, attackAnimatonsList.Length);
                isCharging = false;
            }
        }

        public float EnemyHitDamage()
        {
            return Random.Range(minHitDamage, maxHitDamage);
        }

        void PurposelessEnemyPatrol()
        {
            if (!destinationPointSet)
            {
                float randomX = Random.Range(-walkRange, walkRange);
                float randomZ = Random.Range(-walkRange, walkRange);

                targetVector = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
                destinationPointSet = true;
            }
            else
            {
                agent.SetDestination(targetVector);
            }

            Vector3 distanceToTargetPoint = transform.position - targetVector;
            if (distanceToTargetPoint.magnitude < 0.5f)
            {
                //targetVector = transform.position;
                DOVirtual.Float(animatorSpeed, 0, 0.1f, (speed) => animatorSpeed = speed);
                if (WaitForNothingCoroutine != null)
                {
                    return;
                }
                WaitForNothingCoroutine = StartCoroutine(WaitForNothing());
            }
            if (!Physics.Raycast(targetVector, -transform.up ,2.0f, groundMask))
            {
                destinationPointSet = false;
            }

            IEnumerator WaitForNothing()
            {
                yield return new WaitForSeconds(waitTime);
                destinationPointSet = false;
                WaitForNothingCoroutine = null;
            }
        }

        void GuardianEnemyPatrol()
        {
            if (!destinationPointSet)
            {
                float randomX = Random.Range(-protectedAreaBorderRange, protectedAreaBorderRange);
                float randomZ = Random.Range(-protectedAreaBorderRange, protectedAreaBorderRange);

                if ((randomX > 0 && randomX < resourceAreaBorderRange) || (randomX < 0 && randomX > -resourceAreaBorderRange))
                {
                    return;
                }
                else if((randomZ > 0 && randomZ < resourceAreaBorderRange) || (randomZ < 0 && randomZ > -resourceAreaBorderRange))
                {
                    return;
                }

                targetVector = new Vector3(protectedResource.transform.position.x + randomX, protectedResource.transform.position.y, protectedResource.transform.position.z + randomZ);
                destinationPointSet = true;
            }
            else
            {
                agent.SetDestination(targetVector);
            }

            Vector3 distanceToTargetPoint = transform.position - targetVector;
            if (distanceToTargetPoint.magnitude < 0.5f)
            {
                //targetVector = transform.position;
                DOVirtual.Float(animatorSpeed, 0, 0.1f, (speed) => animatorSpeed = speed);
                if (WaitForNothingCoroutine != null)
                {
                    return;
                }
                WaitForNothingCoroutine = StartCoroutine(WaitForNothing());
            }
            if (!Physics.Raycast(targetVector, -transform.up, 2.0f, groundMask))
            {
                //destinationPointSet = false;
            }

            IEnumerator WaitForNothing()
            {
                yield return new WaitForSeconds(waitTime);
                destinationPointSet = false;
                WaitForNothingCoroutine = null;
            }
        }
        
        void OnPlayerHit(EnemyBehaviours target)
        {
            if (target == this)
            {
                StopEnemyCoroutines();
                DamageCoroutine = StartCoroutine(DamageTakenCoroutine());

                enemyDetector.SetCurrentTarget(null);
                isLockedTarget = false;

                if (!leftClicked)
                {
                    enemyHealth -= Random.Range(25f, 50f);
                }
                else
                {
                    enemyHealth -= characterMovement.WeaponDamage();
                    leftClicked = false;
                }

                Debug.Log(enemyHealth);

                //Damage taken anim
                transform.DOMove(transform.position - (transform.forward / 2), 0.3f).SetDelay(0.1f);
                animator.SetTrigger("DamageTaken");

                StopMoving();
            }
            
            IEnumerator DamageTakenCoroutine()
            {
                isStunned = true;
                yield return new WaitForSeconds(stunnedTimeAfterFamageTaken);
                weaponController.beAbleToAttack = true;
                isStunned = false;                
            }
        }

        void OnPlayerCounter(EnemyBehaviours target)
        {
            if (target == this)
            {
                PrepareAttack(false);
            }
        }

        void OnPlayerLockedToEnemy(EnemyBehaviours target)
        {
            if (target == this)
            {
                StopEnemyCoroutines();

                isLockedTarget = true;
                PrepareAttack(false);
                StopMoving();
            }
        }

        public void StopMoving()
        {
            isMoving = false;
            agent.SetDestination(transform.position);
        }

        public void AttackAnimationPlayingEvent()
        {
            isPlayingAttackAnimation = true;
        } 
        public void AttackAnimationStoppedEvent()
        {
            isPlayingAttackAnimation = false;
        }


        void PrepareAttack(bool active)
        {
            isPraperingAttack = active;

            if (active)
            {
                //counterParticles.Play();
            }
            else
            {
                StopMoving();
                //counterParticles.Stop();
                //counterParticles.Clear();
            }
        }

        void Death()
        {
            StopEnemyCoroutines();
            StopMoving();
            weaponController.beAbleToAttack = true;

            animator.SetBool("Dead", true);
            isDead = true;
        }

        void StopEnemyCoroutines()
        {
            PrepareAttack(false);

            if (isRetreating)
            {
                if (RetreatCoroutine != null)
                {
                    StopCoroutine(RetreatCoroutine);
                }
            }

            if (prepareToAttackCoroutine != null)
            {
                StopCoroutine(prepareToAttackCoroutine);
            }

            if (DamageCoroutine != null)
            {
                StopCoroutine(DamageCoroutine);
            }

            if (MovementCoroutine != null)
            {
                StopCoroutine(MovementCoroutine);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PlayerWeapon") && characterMovement.isAttack && other.gameObject.GetComponent<WeaponController>().beAbleToAttack)
            {
                leftClicked = true;
                OnPlayerHit(this);
                weaponController = other.gameObject.GetComponent<WeaponController>();
                weaponController.beAbleToAttack = false;
            }
        }

        #region Return Public Bools
        public bool IsAttackable()
        {
            return enemyHealth > 0;
        }
        public bool IsPreparingAttack()
        {
            return isPraperingAttack;
        }
        public bool IsRetreanting()
        {
            return isRetreating;
        }
        public bool IsLockedTarget()
        {
            return isLockedTarget;
        }
        public bool IsStunned()
        {
            return isStunned;
        }

        public bool IsOnAttack()
        {
            return isPlayingAttackAnimation;
        }
        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(targetVector, 0.3f);
        }
    }

}
