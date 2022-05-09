using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace UnityEngine.AI.MonsterBehavior
{
    public class EnemyDetector : MonoBehaviour
    {
        public LayerMask layerMask;

        private Vector3 inputDirection;
        private Vector3 forward;
        private Vector3 right;
        
        RaycastHit info;

        private EnemyBehaviours currentTarget;

        GameObject player;
        Camera mainCam;
        CharacterMovement characterMovement;
        Collider[] enemyHitColliders;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            mainCam = Camera.main;
            forward = mainCam.transform.forward;
            right = mainCam.transform.right;
            //forward = player.transform.forward;
            //right = player.transform.right;

            characterMovement = GetComponentInParent<CharacterMovement>();
        }
        
        void Update()
        {
            if (PlayerHealth.dead)
            {
                return;
            }

            forward.y = 0;
            forward.Normalize();

            right.y = 0;
            right.Normalize();

            inputDirection = forward * characterMovement.movementVector.z + right * characterMovement.movementVector.x;
            inputDirection = inputDirection.normalized;

            if (Physics.SphereCast(transform.position, 3f, inputDirection, out info, 10, layerMask))
            {
                if ( info.collider.transform.GetComponent<EnemyBehaviours>() != null && info.collider.transform.GetComponent<EnemyBehaviours>().IsAttackable() && !info.collider.transform.GetComponent<EnemyBehaviours>().IsDead())
                {
                    if (!info.collider.gameObject.GetComponentInParent<EnemyBehaviours>().IsFriend())
                    {
                        currentTarget = info.collider.transform.GetComponent<EnemyBehaviours>();
                    }
                }
            }
            else
            {
                if (currentTarget != null)
                {
                    return;
                }
                enemyHitColliders = Physics.OverlapSphere(transform.position, 5f, layerMask);

                if (enemyHitColliders.Length == 0)
                {
                    return;
                }
                int x = Random.Range(0, enemyHitColliders.Length);
                if (enemyHitColliders[x].gameObject.GetComponent<EnemyBehaviours>() != null && !enemyHitColliders[x].gameObject.GetComponent<EnemyBehaviours>().IsFriend() && !enemyHitColliders[x].gameObject.GetComponent<EnemyBehaviours>().IsDead())
                {
                    currentTarget = enemyHitColliders[x].gameObject.GetComponent<EnemyBehaviours>();
                }
            }

            if (currentTarget != null && Vector3.Distance(transform.position, currentTarget.gameObject.transform.position) > 10f)
            {
                currentTarget = null;
            }
        }

        public void SetCurrentTarget(EnemyBehaviours target)
        {
            currentTarget = target;
        }

        public EnemyBehaviours CurrentTarget()
        {
            return currentTarget;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, inputDirection);
            Gizmos.DrawLine(transform.position, inputDirection * 10);
            Gizmos.DrawWireSphere(transform.position, 1);
            if (CurrentTarget() != null)
            {
                Gizmos.DrawSphere(CurrentTarget().gameObject.transform.position, 0.5f);
            }
        }
    }
}

