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
        [SerializeField] private float movementSpeed = 3.5f;
        [SerializeField] private float SprintSpeed = 5f;
        [SerializeField] private float stunnedTimeAfterFamageTaken = 0.5f;
        [SerializeField] private float maxDistanceToTarget = 10f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackDuration = 0.6f;
        [SerializeField] private float minHitDamage = 5f;
        [SerializeField] private float maxHitDamage = 25f;

        [SerializeField] private string[] attackAnimatonsList;

        [SerializeField] private ParticleSystem counterParticles;

        public UnityEvent<PlayerHealth> HitThePlayer;

        private float distanceToTarget;
        private float currentSpeed;
        private float animatorSpeed;

        private int attackAnimationIndex;

        private bool isPraperingAttack;
        private bool isMoving;
        private bool isRetreating;
        private bool isLockedTarget;
        private bool isStunned;
        private bool isCharging;
        private bool isDead;
        private bool isPlayerInAttackRange;

        private GameObject player;
        private GameObject enemyTarget;

        private Coroutine prepareToAttackCoroutine;
        private Coroutine RetreatCoroutine;
        private Coroutine DamageCoroutine;
        private Coroutine MovementCoroutine;

        private EnemyDetector enemyDetector;
        private PlayerCombat playerCombat;
        private Animator animator;
        private NavMeshAgent agent;

        private Vector3 targetVector;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            enemyDetector = player.GetComponentInChildren<EnemyDetector>();
            playerCombat = player.GetComponent<PlayerCombat>();
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();

            playerCombat.OnDamageTaken.AddListener((x) => OnPlayerHit(x));
            playerCombat.OnCounterAttack.AddListener((x) => OnPlayerCounter(x));
            playerCombat.OnLockedToEnemy.AddListener((x) => OnPlayerLockedToEnemy(x));

            enemyTarget = player;
        }

        void Update()
        {
            targetVector = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            agent.speed = currentSpeed;
            animator.SetFloat("Speed", animatorSpeed);

            if (!isDead)
            {
                transform.LookAt(targetVector);
                EnemyMovement();
            }
        }

        void EnemyMovement()
        {
            distanceToTarget = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToTarget <= maxDistanceToTarget && distanceToTarget > attackRange)
            {
                agent.SetDestination(targetVector);
                currentSpeed = movementSpeed;

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
                currentSpeed = SprintSpeed;
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

        void OnPlayerHit(EnemyBehaviours target)
        {
            if (target == this)
            {
                StopEnemyCoroutines();
                DamageCoroutine = StartCoroutine(DamageTakenCoroutine());

                enemyDetector.SetCurrentTarget(null);
                isLockedTarget = false;

                enemyHealth -= Random.Range(5f, 25f);
                Debug.Log(enemyHealth);
                if (enemyHealth <= 0)
                {
                    Death();
                    return;
                }

                //Damage taken anim
                transform.DOMove(transform.position - (transform.forward / 2), 0.3f).SetDelay(0.1f);
                animator.SetTrigger("DamageTaken");

                StopMoving();
            }
            
            IEnumerator DamageTakenCoroutine()
            {
                isStunned = true;
                yield return new WaitForSeconds(stunnedTimeAfterFamageTaken);
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
        #endregion
    }

}
