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
            forward.y = 0;
            forward.Normalize();

            right.y = 0;
            right.Normalize();

            inputDirection = forward * characterMovement.movementVector.z + right * characterMovement.movementVector.x;
            inputDirection = inputDirection.normalized;

            if (Physics.SphereCast(transform.position, 3f, inputDirection, out info, 10, layerMask))
            {
                if (info.collider.transform.GetComponent<EnemyBehaviours>().IsAttackable())
                {
                    currentTarget = info.collider.transform.GetComponent<EnemyBehaviours>();
                }
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

            Gizmos.DrawWireSphere(transform.position, 1);
            if (CurrentTarget() != null)
            {
                Gizmos.DrawSphere(CurrentTarget().gameObject.transform.position, 0.5f);
            }
        }
    }
}

