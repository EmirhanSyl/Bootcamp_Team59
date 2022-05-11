using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI.MonsterBehavior;

public class PlayerHealth : MonoBehaviour
{
    public static float health;

    public static bool isHitted;
    public static bool dead;

    public float healthOnInspector = 100f;

    [SerializeField] private float noTakeDamageTime = 0.5f;

    private float currentHealth;

    private bool godMode;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();

        health = healthOnInspector;
        currentHealth = health;
    }

    
    void Update()
    {
        if (health != currentHealth && currentHealth > 0)
        {
            animator.SetTrigger("DamageTaken");
            currentHealth = health;
        }
        else if (currentHealth <= 0)
        {
            animator.SetTrigger("Dead");
            dead = true;
        }

        if (CharacterMovement.Skeleton)
        {
            currentHealth = 20;
            health = currentHealth;
        }
    }
    public void DamageTakenAnimPlaying()
    {
        isHitted = true;
    }
    public void DamageTakenAnimStopped()
    {
        isHitted = false;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("EnemyWeapon"))
    //    {
    //        EnemyBehaviours enemy = collision.gameObject.GetComponentInParent<EnemyBehaviours>();
    //        health -= enemy.EnemyHitDamage();
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        EnemyBehaviours enemyScript = null;
        if (other.CompareTag("EnemyWeapon"))
        {
            enemyScript = other.GetComponentInParent<EnemyBehaviours>();
        }
        if (enemyScript != null && enemyScript.regionDrowpdown != EnemyBehaviours.Region.Forest && enemyScript.targetObject == gameObject && enemyScript.IsOnAttack())
        {
            EnemyBehaviours enemy = other.gameObject.GetComponentInParent<EnemyBehaviours>();
            if (enemy.IsOnAttack() && !godMode)
            {
                health -= enemy.EnemyHitDamage();
                StartCoroutine(NoTakeDamageCoroutine());
            }
        }
    }

    IEnumerator NoTakeDamageCoroutine()
    {
        godMode = true;
        yield return new WaitForSeconds(noTakeDamageTime);
        godMode = false;
        StopCoroutine(NoTakeDamageCoroutine());
    }

}
