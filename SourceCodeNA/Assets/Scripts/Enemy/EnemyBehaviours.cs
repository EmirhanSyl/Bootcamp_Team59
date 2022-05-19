using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using DG.Tweening;

namespace UnityEngine.AI.MonsterBehavior
{
    public class EnemyBehaviours : MonoBehaviour
    {
        [Header("Enemy States")]
        [SerializeField] private float enemyHealth = 100f;
        [SerializeField] private float movementSpeed = 1.5f;
        [SerializeField] private float SprintSpeed = 3.5f;
        [SerializeField] private float stunnedTimeAfterFamageTaken = 0.5f;
        [SerializeField] private float knocbackDevider = 1f;
        public float maxDistanceToTarget = 10f;
        [Header("Attack Stats")]
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackDuration = 0.6f;
        [SerializeField] private float minHitDamage = 5f;
        [SerializeField] private float maxHitDamage = 25f;

        [SerializeField] private string[] attackAnimatonsList;

        [SerializeField] private ParticleSystem counterParticles;
        [SerializeField] private ParticleSystem bloodParticles;

        [SerializeField] private LayerMask enemyLayers;

        public enum EnemyStateType { Purposeless, Guardian, TowerWizard, RobotWorker };
        [Header("Enemy Type Informations")]public EnemyStateType enemyStateTypeDropdown;

        public enum Region {Desert, Forest, Ice };
        [Header("Region Informations")]public Region regionDrowpdown;

        public LayerMask groundMask;

        [HideInInspector]public UnityEvent<PlayerHealth> HitThePlayer;

        public float rotatioSpeed = 50f;
        public bool controllingByGroupManager;
        public bool hittedByPlayer;
        public bool friendWithPlayer;

        public float distanceToTarget;
        public float poisonedTakeDamageDuration;
        public float poisonedHitCount;

        private float poisonedTakeDamageTimer;
        private float currentSpeed;
        private float animatorSpeed;
        private float destinationRestarter = 5;
        private float minDist = Mathf.Infinity;

        private int attackAnimationIndex;

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
        private bool stopBlood;
        private bool damageTakenAnimIsPlaying;
        private bool dodgenAnimIsPlaying;
        private bool deadForOneTime;
        private bool poisenedAlready;

        //Purposeless Enemy
        [SerializeField] private float walkRange = 5;
        [SerializeField] private float waitTime = 3;
        [SerializeField] private float dodgeChance = 0.2f; 

        private float currentWaitedTime;

        private bool destinationPointSet;
        //Guardian Enemy
        public GameObject protectedResource;
        [SerializeField] private float resourceAreaBorderRange;
        [SerializeField] private float protectedAreaBorderRange;

        //TowerWizard
        [SerializeField] private GameObject projectile;
        [SerializeField] private Transform projectileInitLocation;

        //Robot Worker
        [SerializeField] private string[] collectAnimatonsList;
        [SerializeField] private float carryingCapacity;

        private float collectedResource;
        private int collectAnimationIndex;
        private bool isGetheringResource;
        public bool resourceCompletelyExploited;


        private GameObject player;
        private Collider[] enemyColls;

        private Coroutine prepareToAttackCoroutine;
        private Coroutine RetreatCoroutine;
        private Coroutine DamageCoroutine;
        private Coroutine MovementCoroutine;
        private Coroutine WaitForNothingCoroutine;
        private Coroutine CannotArriveToDestinationCoroutine;

        private EnemyDetector enemyDetector;
        private PlayerCombat playerCombat;
        private CharacterMovement characterMovement;
        private WeaponController weaponController;
        private Animator animator;
        private NavMeshAgent agent;
        private NPCGroupManager groupController;

        public GameObject targetObject;
        public Vector3 targetVector;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            
            enemyDetector = player.GetComponentInChildren<EnemyDetector>();
            characterMovement = player.GetComponent<CharacterMovement>();
            playerCombat = player.GetComponent<PlayerCombat>();
            animator = GetComponent<Animator>();
            if (enemyStateTypeDropdown != EnemyStateType.TowerWizard)
            {
                agent = GetComponent<NavMeshAgent>();
            }
            else
            {
                controllingByGroupManager = true;
            }

            playerCombat.OnDamageTaken.AddListener((x) => OnPlayerHit(x));
            playerCombat.OnCounterAttack.AddListener((x) => OnPlayerCounter(x));
            playerCombat.OnLockedToEnemy.AddListener((x) => OnPlayerLockedToEnemy(x));


            if (regionDrowpdown == Region.Forest)
            {
                friendWithPlayer = true;
            }

            switch (regionDrowpdown)
            {
                case Region.Desert:
                    gameObject.transform.GetChild(2).gameObject.layer = 8;
                    break;
                case Region.Forest:
                    gameObject.transform.GetChild(2).gameObject.layer = 9;
                    break;
                case Region.Ice:
                    gameObject.transform.GetChild(2).gameObject.layer = 10;
                    break;

            }

            if (transform.parent.gameObject.GetComponent<NPCGroupManager>() != null)
            {
                controllingByGroupManager = true;
            }
            if (controllingByGroupManager)
            {
                groupController = GetComponentInParent<NPCGroupManager>();
                enemyLayers = groupController.enemyMask;
            }

        }

        void Update()
        {
            if (enemyStateTypeDropdown != EnemyStateType.TowerWizard)
            {
                agent.SetDestination(targetVector);
                agent.speed = currentSpeed;
                if (!isDead)
                {
                    animator.SetFloat("Speed", animatorSpeed);
                }
            }

            if (!isDead)
            {
                LockToTheTarget();
                //transform.LookAt(targetVector);
                //var rotation = Quaternion.LookRotation(targetVector - transform.position, Vector3.up);
                //rotation.y = 0;
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotatioSpeed * Time.deltaTime);
                transform.DOLookAt(new Vector3(targetVector.x, transform.position.y, targetVector.z), 0.5f);
                if (enemyStateTypeDropdown != EnemyStateType.TowerWizard)
                {
                    EnemyMovement();
                }
                else
                {
                    TowerWizard();
                }
            }
            //else
            //{
            //    animator.SetBool("Dead", true);
            //}
            if (enemyHealth <= 0)
            {
                Death();
                animator.SetBool("Dead", true);
                Destroy(gameObject, 4);
                StartCoroutine(DeadPart());
                if (!deadForOneTime)
                {
                    groupController.SomebodyDied();
                    deadForOneTime = true;
                }
                IEnumerator DeadPart()
                {
                    yield return new WaitForSeconds(3.9f);
                    if (!stopBlood)
                    {
                        Instantiate(bloodParticles, transform.position, Quaternion.identity);
                        stopBlood = true;
                    }
                }
            }
            if (poisenedAlready)
            {
                GetPoisoned();
            }

        }
        void LockToTheTarget()
        {
            //if (targetObject != null)
            //{
            //    return;
            //}

            if (Physics.CheckSphere(gameObject.transform.position, maxDistanceToTarget, enemyLayers))
            {
                //enemyColls = Physics.OverlapSphere(gameObject.transform.position, maxDistanceToTarget, enemyLayers);
               
                float distanceToClosestEnemy = Mathf.Infinity;
                GameObject closestEnemy = null;
                Collider[] allEnemies = Physics.OverlapSphere(gameObject.transform.position, maxDistanceToTarget, enemyLayers);

                foreach (var currentEnemyColl in allEnemies)
                {
                    if ((currentEnemyColl.gameObject.transform.parent.gameObject.CompareTag("Player") && PlayerHealth.dead) || (currentEnemyColl.GetComponentInParent<EnemyBehaviours>() != null && currentEnemyColl.GetComponentInParent<EnemyBehaviours>().IsDead()))
                    {
                        targetObject = null;
                        return;
                    }
                    float distanceToCurrentEnemy = (currentEnemyColl.gameObject.transform.position - transform.position).sqrMagnitude;
                    if (distanceToCurrentEnemy < distanceToClosestEnemy)
                    {
                        distanceToClosestEnemy = distanceToCurrentEnemy;
                        closestEnemy = currentEnemyColl.gameObject.transform.parent.gameObject;
                        targetObject = closestEnemy;
                        targetVector = targetObject.transform.position;
                        distanceToTarget = Vector3.Distance(transform.position, targetVector);
                    }
                    //if ((targetObject.gameObject.CompareTag("Player") && PlayerHealth.dead) || (targetObject.GetComponent<EnemyBehaviours>() != null && targetObject.GetComponent<EnemyBehaviours>().IsDead()))
                    //{
                    //    targetObject = null;
                    //    return;
                    //}

                    if (!IsDead())
                    {
                        gameObject.transform.LookAt(new Vector3(targetVector.x, gameObject.transform.position.y, targetVector.z));
                    }
                }
            }
            else
            {
                distanceToTarget = maxDistanceToTarget + 1;
            }

        }

        void EnemyMovement()
        {

            if (distanceToTarget <= maxDistanceToTarget && distanceToTarget > attackRange)
            {            

                DOVirtual.Float(currentSpeed, SprintSpeed, 0.4f, (speed) => currentSpeed = speed);
                DOVirtual.Float(animatorSpeed, 1, 0.1f, (speed) => animatorSpeed = speed);

                isPlayerInAttackRange = false;
            }
            else if(distanceToTarget < attackRange)
            {
                if (targetObject == null)
                {
                    return;
                }

                DOVirtual.Float(animatorSpeed, 0, 0.1f, (speed) => animatorSpeed = speed);
                isPlayerInAttackRange = true;
                if (!damageTakenAnimIsPlaying)
                {
                    if ((targetObject.gameObject.CompareTag("Player") && PlayerHealth.dead) || (targetObject.GetComponent<EnemyBehaviours>() != null && targetObject.GetComponent<EnemyBehaviours>().IsDead()))
                    {
                        return;
                    }
                    EnemyAttack();
                }
                
            }
            else
            {
                isPlayerInAttackRange = false;
                DOVirtual.Float(animatorSpeed, 0.5f, 0.1f, (speed) => animatorSpeed = speed);
                DOVirtual.Float(currentSpeed, movementSpeed, 0.4f, (speed) => currentSpeed = speed);

                if (controllingByGroupManager && groupController.questDone)
                {
                    targetObject = groupController.castleLocation;
                    targetVector = targetObject.transform.position;
                    return;
                }
                switch ((int)enemyStateTypeDropdown)
                {
                    case 0:
                        PurposelessEnemyPatrol();
                        break;
                    case 1:
                        GuardianEnemyPatrol();
                        break;
                    case 3:
                        RobotWorker();
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
                if (!isDead)
                {
                    animator.SetTrigger(attackAnimatonsList[attackAnimationIndex]);
                    attackAnimationIndex = (int)Mathf.Repeat(attackAnimationIndex + 1, attackAnimatonsList.Length);
                }
                if (enemyStateTypeDropdown == EnemyStateType.TowerWizard)
                {
                    var initiatedProjectile = Instantiate(projectile, projectileInitLocation.position, Quaternion.identity);
                    initiatedProjectile.GetComponent<ProjectileMovement>().ThrowProjectile(targetObject);
                }
                else if(enemyStateTypeDropdown != EnemyStateType.TowerWizard && targetObject.GetComponent<EnemyBehaviours>() != null)
                {
                    OnNPCCounter();
                }
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

            Vector3 distanceToTargetPoint = transform.position - targetVector;
            if (distanceToTargetPoint.magnitude < 0.5f)
            {
                targetVector = transform.position;
                DOVirtual.Float(animatorSpeed, 0, 0.1f, (speed) => animatorSpeed = speed);
                if (WaitForNothingCoroutine != null)
                {
                    return;
                }
                WaitForNothingCoroutine = StartCoroutine(WaitForNothing());
            }
            else
            {
                destinationRestarter -= Time.deltaTime;
                if (destinationRestarter < 0)
                {
                    destinationPointSet = false;
                    destinationRestarter = 5;
                }
            }            

            IEnumerator WaitForNothing()
            {
                yield return new WaitForSeconds(waitTime);
                destinationPointSet = false;
                destinationRestarter = 5;
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
                //agent.SetDestination(targetVector);
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
            else if(distanceToTargetPoint.magnitude >= 0.5f && distanceToTargetPoint.magnitude < 7f)
            {
                destinationRestarter -= Time.deltaTime;
                if (destinationRestarter < 0)
                {
                    destinationPointSet = false;
                    destinationRestarter = 5;
                }
            }

            IEnumerator WaitForNothing()
            {
                yield return new WaitForSeconds(waitTime);
                destinationPointSet = false;
                WaitForNothingCoroutine = null;
            }
        }
        
        void TowerWizard()
        {
            if (targetVector == null)
            {
                return;
            }
            if (distanceToTarget < attackRange)
            {
                if ((targetObject.gameObject.CompareTag("Player") && PlayerHealth.dead) || (targetObject.GetComponent<EnemyBehaviours>() != null && targetObject.GetComponent<EnemyBehaviours>().IsDead()))
                {
                    return;
                }

                EnemyAttack();
            }            
        }


        void RobotWorker()
        {
            if (collectedResource >= carryingCapacity || resourceCompletelyExploited)
            {
                groupController.questDone = true;
            }
            if (!destinationPointSet)
            {
                float randomX = Random.Range(-resourceAreaBorderRange, resourceAreaBorderRange);
                float randomZ = Random.Range(-resourceAreaBorderRange, resourceAreaBorderRange);

                targetVector = new Vector3(protectedResource.transform.position.x + randomX, protectedResource.transform.position.y, protectedResource.transform.position.z + randomZ);
                destinationPointSet = true;
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
            else if (distanceToTargetPoint.magnitude >= 0.5f && distanceToTargetPoint.magnitude < 7f)
            {
                destinationRestarter -= Time.deltaTime;
                if (destinationRestarter < 0)
                {
                    destinationPointSet = false;
                    destinationRestarter = 5;
                }
            }

            IEnumerator WaitForNothing()
            {
                animator.SetTrigger(collectAnimatonsList[collectAnimationIndex]);
                collectAnimationIndex = (int)Mathf.Repeat(collectAnimationIndex + 1, collectAnimatonsList.Length);
                yield return new WaitForSeconds(waitTime);
                destinationPointSet = false;
                WaitForNothingCoroutine = null;
            }
        }

        public void OnNPCCounter()
        {
            float dodgeFloat = Random.value;
            if (dodgeFloat < dodgeChance)
            {
                EnemyBehaviours targetScript = null;
                if (targetObject.GetComponent<EnemyBehaviours>() != null)
                {
                    targetScript = targetObject.GetComponent<EnemyBehaviours>();
                }

                if (targetScript != null && targetScript.isPlayingAttackAnimation && targetScript.targetObject == gameObject)
                {
                    animator.SetTrigger("Dodge");
                    transform.DOMove(transform.position - (new Vector3(transform.right.x + 2f, 0f, transform.right.z)), 1f).SetDelay(0.2f);
                }
                else if (targetObject.CompareTag("Player"))
                {
                    animator.SetTrigger("Dodge");
                    transform.DOMove(transform.position - (new Vector3(transform.right.x + 2f, 0f, transform.right.z)), 1f).SetDelay(0.2f);
                }
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

                if (hittedByPlayer)
                {
                    if (!leftClicked)
                    {
                        enemyHealth -= Random.Range(25f, 50f);
                    }
                    else
                    {
                        enemyHealth -= characterMovement.WeaponDamage();
                        leftClicked = false;
                    }
                    hittedByPlayer = false;
                }

                Debug.Log(enemyHealth);

                //Damage taken anim
                transform.DOMove(transform.position - (transform.forward / knocbackDevider), 0.3f).SetDelay(0.1f);

                if (!isDead)
                {
                    animator.SetTrigger("DamageTaken");
                }

                StopMoving();
            }
            
            IEnumerator DamageTakenCoroutine()
            {
                isStunned = true;
                yield return new WaitForSeconds(stunnedTimeAfterFamageTaken);
                if (weaponController != null)
                {
                    weaponController.beAbleToAttack = true;
                }
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
            if (enemyStateTypeDropdown != EnemyStateType.TowerWizard)
            {
                agent.SetDestination(transform.position);
            }
        }

        void GetPoisoned()
        {
            poisonedTakeDamageTimer += Time.deltaTime;
            if (poisonedTakeDamageTimer >= poisonedTakeDamageDuration && poisonedHitCount > 0)
            {
                GetHittedFromNPC(Random.Range(5f, 10f));
                poisonedHitCount--;
                poisonedTakeDamageTimer = 0f;
            }
            if (poisonedHitCount == 0)
            {
                poisenedAlready = false;
                poisonedHitCount = 3;
            }
        }

        void GetShocked()
        {

        }

        void Healing()
        {

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
            if (weaponController != null)
            {
                weaponController.beAbleToAttack = true;
            }

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
            if (other.gameObject.CompareTag("PlayerWeapon") && characterMovement.isAttack && other.gameObject.GetComponent<WeaponController>().beAbleToAttack && !friendWithPlayer)
            {
                leftClicked = true;
                OnPlayerHit(this);
                if (other.gameObject.GetComponent<WeaponController>() != null)
                {
                    weaponController = other.gameObject.GetComponent<WeaponController>();
                    weaponController.beAbleToAttack = false;
                }
                hittedByPlayer = true;
            }
            if (other.gameObject.CompareTag("Poison") && !friendWithPlayer && !poisenedAlready)
            {
                GetHittedFromNPC(Random.Range(5f, 10f));
                poisonedHitCount--;
                poisenedAlready = true;
            }
            else if(other.gameObject.CompareTag("ElectricMagic") && !friendWithPlayer)
            {

            }
            else if (other.gameObject.CompareTag("HealMagic") && friendWithPlayer)
            {

            }
        }

        public void GetHittedFromNPC(float damage)
        {
            enemyHealth -= damage;
            OnPlayerHit(this);
        }

        public void GetHitAnimPlaying()
        {
            damageTakenAnimIsPlaying = true;
        }
        public void GetHitAnimStopped()
        {
            damageTakenAnimIsPlaying = false;
        }
        public void DodgeStarted()
        {
            dodgenAnimIsPlaying = true;
        }
        public void DodgeFinished()
        {
            dodgenAnimIsPlaying = false;
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
        public bool IsOnDodge()
        {
            return dodgenAnimIsPlaying;
        }
        public bool IsAttacking()
        {
            return isPlayerInAttackRange;
        }
        public bool IsFriend()
        {
            return friendWithPlayer;
        }
        public bool IsDead()
        {
            return isDead;
        }
        #endregion        
        public void BackupAwake()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            enemyDetector = player.GetComponentInChildren<EnemyDetector>();
            characterMovement = player.GetComponent<CharacterMovement>();
            playerCombat = player.GetComponent<PlayerCombat>();
            animator = GetComponent<Animator>();
            if (enemyStateTypeDropdown != EnemyStateType.TowerWizard)
            {
                agent = GetComponent<NavMeshAgent>();
            }
            else
            {
                controllingByGroupManager = true;
            }

            playerCombat.OnDamageTaken.AddListener((x) => OnPlayerHit(x));
            playerCombat.OnCounterAttack.AddListener((x) => OnPlayerCounter(x));
            playerCombat.OnLockedToEnemy.AddListener((x) => OnPlayerLockedToEnemy(x));

            if (regionDrowpdown == Region.Forest)
            {
                friendWithPlayer = true;
            }

            switch (regionDrowpdown)
            {
                case Region.Desert:
                    gameObject.transform.GetChild(2).gameObject.layer = 8;
                    break;
                case Region.Forest:
                    gameObject.transform.GetChild(2).gameObject.layer = 9;
                    break;
                case Region.Ice:
                    gameObject.transform.GetChild(2).gameObject.layer = 10;
                    break;

            }

            if (transform.parent.gameObject.GetComponent<NPCGroupManager>() != null)
            {
                controllingByGroupManager = true;
            }
            if (controllingByGroupManager)
            {
                groupController = GetComponentInParent<NPCGroupManager>();
                enemyLayers = groupController.enemyMask;
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(targetVector, 0.3f);
        }
    }

}
