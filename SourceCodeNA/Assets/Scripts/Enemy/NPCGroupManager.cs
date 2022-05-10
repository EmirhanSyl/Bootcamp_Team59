using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI.MonsterBehavior;

public class NPCGroupManager : MonoBehaviour
{    
    public EnemyBehaviours.Region groupRegionDropdown;

    public LayerMask enemyMask;

    private EnemyBehaviours[] NPCGroup;
    public Collider[] enemyHitColliders;

    private GameObject player;

    void Start()
    {
        NPCGroup = new EnemyBehaviours[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            NPCGroup[i] = transform.GetChild(i).gameObject.GetComponent<EnemyBehaviours>();

            NPCGroup[i].regionDrowpdown = groupRegionDropdown;
            NPCGroup[i].controllingByGroupManager = true;

            if (NPCGroup[i].regionDrowpdown == EnemyBehaviours.Region.Forest)
            {
                NPCGroup[i].friendWithPlayer = true;
            }

            switch (NPCGroup[i].regionDrowpdown)
            {
                case EnemyBehaviours.Region.Desert:
                    NPCGroup[i].gameObject.transform.GetChild(2).gameObject.layer = 8;
                    break;
                case EnemyBehaviours.Region.Forest:
                    NPCGroup[i].gameObject.transform.GetChild(2).gameObject.layer = 9;
                    break;
                case EnemyBehaviours.Region.Ice:
                    NPCGroup[i].gameObject.transform.GetChild(2).gameObject.layer = 10;
                    break;
            }
        }

        player = GameObject.FindGameObjectWithTag("Player");
    }

    
    void Update()
    {
        //foreach (EnemyBehaviours NPC in NPCGroup)
        //{               
        //    if (Physics.CheckSphere(NPC.gameObject.transform.position, NPC.maxDistanceToTarget, enemyMask))
        //    {
        //        enemyHitColliders = Physics.OverlapSphere(NPC.gameObject.transform.position, NPC.maxDistanceToTarget, enemyMask);
        //        NPC.targetObject = enemyHitColliders[enemyHitColliders.Length - 1].gameObject;
        //        NPC.targetVector = enemyHitColliders[enemyHitColliders.Length - 1].gameObject.transform.position;
        //        NPC.distanceToTarget = Vector3.Distance(enemyHitColliders[enemyHitColliders.Length - 1].gameObject.transform.position, NPC.gameObject.transform.position);
        //        NPC.rotatioSpeed = 200f;
        //        if ((NPC.targetObject.transform.parent.gameObject.CompareTag("Player") && PlayerHealth.dead) || (NPC.targetObject.GetComponentInParent<EnemyBehaviours>() != null && NPC.targetObject.GetComponentInParent<EnemyBehaviours>().IsDead()))
        //        {
        //            return;
        //        }
        //        if (!NPC.IsDead())
        //        {
        //            NPC.gameObject.transform.LookAt(new Vector3(NPC.targetVector.x, NPC.gameObject.transform.position.y, NPC.targetVector.z));
        //        }
        //        if (NPC.IsAttacking())
        //        {
        //            Attack(NPC);
        //        }
        //    }
        //    else
        //    {
        //        NPC.distanceToTarget = NPC.maxDistanceToTarget + 1;
        //    }

        //    if (enemyHitColliders != null && enemyHitColliders.Length != 0 && NPC.targetObject == null)
        //    {
        //        enemyHitColliders = Physics.OverlapSphere(NPC.gameObject.transform.position, NPC.maxDistanceToTarget, enemyMask);
        //        NPC.targetObject = enemyHitColliders[enemyHitColliders.Length - 1].gameObject;
        //        NPC.targetVector = enemyHitColliders[enemyHitColliders.Length - 1].gameObject.transform.position;
        //        NPC.distanceToTarget = Vector3.Distance(enemyHitColliders[enemyHitColliders.Length - 1].gameObject.transform.position, NPC.gameObject.transform.position);
        //        NPC.rotatioSpeed = 200f;

        //        Debug.Log("Burada biþiler oldu!");
        //    }
        //}
    }

    //void Attack(EnemyBehaviours target)
    //{

    //}
}
