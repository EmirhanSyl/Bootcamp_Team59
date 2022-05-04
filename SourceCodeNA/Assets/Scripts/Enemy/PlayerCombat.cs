using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.AI.MonsterBehavior;

public class PlayerCombat : MonoBehaviour
{

    private EnemyDetector enemyDetector;

    public UnityEvent<EnemyBehaviours> OnLockedToEnemy;
    public UnityEvent<EnemyBehaviours> OnDamageTaken;
    public UnityEvent<EnemyBehaviours> OnCounterAttack;

    void Awake()
    {
        enemyDetector = GetComponentInChildren<EnemyDetector>();
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            MoveTowardsTarget(enemyDetector.CurrentTarget(), 0.5f);
        }
    }

    void MoveTowardsTarget(EnemyBehaviours target, float duration)
    {
        OnDamageTaken.Invoke(target);
        transform.DOLookAt(target.transform.position, 0.2f);
        transform.DOMove(TargetOffset(target.transform), duration);
    }

    public Vector3 TargetOffset(Transform target)
    {
        Vector3 position;
        position = target.position;
        return Vector3.MoveTowards(position, transform.position, 0.9f);
    }
}
