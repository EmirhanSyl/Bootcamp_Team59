using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI.MonsterBehavior;

public class NPCWeapon : MonoBehaviour
{
    [SerializeField] private float minAttackDamage;
    [SerializeField] private float maxAttackDamage;
    public GameObject fdsgk;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == gameObject.GetComponentInParent<EnemyBehaviours>().targetObject && gameObject.GetComponentInParent<EnemyBehaviours>().IsAttacking())
        {
            other.gameObject.transform.parent.GetComponent<EnemyBehaviours>().GetHittedFromNPC(Random.Range(minAttackDamage, maxAttackDamage));
        }
    }
}
