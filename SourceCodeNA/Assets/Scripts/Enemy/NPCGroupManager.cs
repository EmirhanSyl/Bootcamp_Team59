using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI.MonsterBehavior;

public class NPCGroupManager : MonoBehaviour
{    
    public EnemyBehaviours.Region groupRegionDropdown;

    public LayerMask enemyMask;

    private EnemyBehaviours[] NPCGroup;
    private Collider[] enemyHitColliders;

    private GameObject player;

    void Start()
    {
        NPCGroup = new EnemyBehaviours[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            NPCGroup[i] = transform.GetChild(i).gameObject.GetComponent<EnemyBehaviours>();

            NPCGroup[i].regionDrowpdown = groupRegionDropdown;
            NPCGroup[i].controllingByGroupManager = true;
        }

        player = GameObject.FindGameObjectWithTag("Player");
    }

    
    void Update()
    {
        foreach (EnemyBehaviours NPC in NPCGroup)
        {               
            if (Physics.CheckSphere(NPC.gameObject.transform.position, 10f, enemyMask))
            {
                enemyHitColliders = Physics.OverlapSphere(NPC.gameObject.transform.position, NPC.maxDistanceToTarget, enemyMask);
                NPC.targetObject = enemyHitColliders[enemyHitColliders.Length - 1].gameObject;
                NPC.targetVector = enemyHitColliders[enemyHitColliders.Length - 1].gameObject.transform.position;
                NPC.distanceToTarget = Vector3.Distance(enemyHitColliders[enemyHitColliders.Length - 1].gameObject.transform.position, NPC.gameObject.transform.position);
                NPC.rotatioSpeed = 200f;
                NPC.gameObject.transform.LookAt(new Vector3(NPC.targetVector.x, NPC.gameObject.transform.position.y, NPC.targetVector.z));
                if (NPC.IsAttacking())
                {
                    Attack(NPC);
                }
            }
            else
            {
                NPC.distanceToTarget = Vector3.Distance(player.transform.position, NPC.gameObject.transform.position);
            }
        }
    }

    void Attack(EnemyBehaviours target)
    {

    }
}
