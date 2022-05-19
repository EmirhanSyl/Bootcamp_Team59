using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.AI.MonsterBehavior;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float moveToEnemyDuration = 0.5f;

    [SerializeField] private string[] attackAnimations;

    [Space(20)]
    public UnityEvent<EnemyBehaviours> OnLockedToEnemy;
    public UnityEvent<EnemyBehaviours> OnDamageTaken;
    public UnityEvent<EnemyBehaviours> OnCounterAttack;

    //Private Variables
    private int animationTypeID;

    public bool isAttackingEnemy;
    public bool isCountering;
    public bool playingCombatAnim;

    private bool turnedIntoASkeleton;

    private Animator animator;
    private EnemyDetector enemyDetector;
    private EnemyBehaviours locedTarget;
    private CharacterMovement characterMovement;

    private Coroutine attackCoroutine;

    void Awake()
    {
        enemyDetector = GetComponentInChildren<EnemyDetector>();
        characterMovement = GetComponent<CharacterMovement>();
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            AttackCheck();
        }
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Dodge();
        }
        if (CharacterMovement.Skeleton && !turnedIntoASkeleton)
        {
            animator = transform.GetChild(1).gameObject.GetComponent<Animator>();
            turnedIntoASkeleton = true;
        }
    }

    void AttackCheck()
    {
        if (isAttackingEnemy)
        {
            return;
        }

        locedTarget = enemyDetector.CurrentTarget();        

        Attack(locedTarget);
    }

    void Attack(EnemyBehaviours target)
    {
        if (target == null)
        {
            return;
        }
        animationTypeID = (int)Mathf.Repeat(animationTypeID + 1, attackAnimations.Length);
        AttackType(attackAnimations[animationTypeID], moveToEnemyDuration, target, moveToEnemyDuration);
        
    }

    void AttackType(string attackName, float cooldown, EnemyBehaviours target, float movementDuration)
    {
        animator.SetTrigger(attackName);

        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine(AttackCoroutine(cooldown));

        if (target == null)
        {
            return;
        }

        target.StopMoving();
        MoveTowardsTarget(target, movementDuration);

        IEnumerator AttackCoroutine(float duration)
        {
            isAttackingEnemy = true;
            yield return new WaitForSeconds(duration);
            isAttackingEnemy = false;
        }
    }

    void MoveTowardsTarget(EnemyBehaviours target, float duration)
    {
        OnLockedToEnemy.Invoke(target);
        transform.DOLookAt(target.transform.position, 0.2f);
        transform.DOMove(TargetOffset(target.transform), duration);
    }

    public Vector3 TargetOffset(Transform target)
    {
        Vector3 position;
        position = target.position;
        return Vector3.MoveTowards(position, transform.position, 0.9f);
    }

    public void HitEvent()
    {
        if (true)
        {
            if (locedTarget == null)
            {
                return;
            }
            if (!locedTarget.IsOnDodge())
            {
                OnDamageTaken.Invoke(locedTarget);
            }
            //Particle codes
            locedTarget.hittedByPlayer = true;
            playingCombatAnim = false;
        }
        
    }

    void Dodge()
    {
        if (PlayerHealth.isHitted || isAttackingEnemy || characterMovement.isAttack || PlayerHealth.dead)
        {
            return;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("Dodge");
            transform.DOMove(transform.position + (transform.right * 5), 0.8f).SetDelay(0.2f);
        }
    }

    public void OnGoingToEnemy()
    {        
        if (!playingCombatAnim)
        {
            locedTarget.OnNPCCounter();
            playingCombatAnim = true;
        }
    }
}
