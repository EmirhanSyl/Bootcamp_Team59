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
        [SerializeField] private float stunnedTimeAfterFamageTaken = 0.5f;

        [SerializeField] private ParticleSystem counterParticles;        

        private bool isPraperingAttack;
        private bool isMoving;
        private bool isRetreating;
        private bool isLockedTarget;
        private bool isStunned;
        private bool isWaiting = true;
        private bool isDead;

        private GameObject player;

        private Coroutine prepareToAttackCoroutine;
        private Coroutine RetreatCoroutine;
        private Coroutine DamageCoroutine;
        private Coroutine MovementCoroutine;

        private EnemyDetector enemyDetector;
        private PlayerCombat playerCombat;
        private Animator animator;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            enemyDetector = player.GetComponentInChildren<EnemyDetector>();
            playerCombat = player.GetComponent<PlayerCombat>();
            animator = GetComponent<Animator>();

            playerCombat.OnDamageTaken.AddListener((x) => OnPlayerHit(x));
            playerCombat.OnCounterAttack.AddListener((x) => OnPlayerCounter(x));
            playerCombat.OnLockedToEnemy.AddListener((x) => OnPlayerLockedToEnemy(x));
        }

        void Update()
        {
            if (!isDead)
            {
                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
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
            //Stop Enemy
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

            animator.SetTrigger("Dead");
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
