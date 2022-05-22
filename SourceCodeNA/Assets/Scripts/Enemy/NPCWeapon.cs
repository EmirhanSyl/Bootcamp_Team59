using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI.MonsterBehavior;

public class NPCWeapon : MonoBehaviour
{
    [SerializeField] private float minAttackDamage;
    [SerializeField] private float maxAttackDamage;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject == gameObject.GetComponentInParent<EnemyBehaviours>().targetObject && gameObject.GetComponentInParent<EnemyBehaviours>().IsAttacking() && !other.gameObject.GetComponent<EnemyBehaviours>().IsOnDodge() && !other.gameObject.GetComponent<EnemyBehaviours>().InHeavyAttack())
        {
            other.gameObject.GetComponent<EnemyBehaviours>().GetHittedFromNPC(Random.Range(minAttackDamage, maxAttackDamage));
        }
    }
}
