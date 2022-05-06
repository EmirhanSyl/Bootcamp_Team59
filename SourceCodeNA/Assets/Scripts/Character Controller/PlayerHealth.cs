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

    private float currentHealth;

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

        if (Input.GetMouseButtonDown(0) && !dead)
        {
            health -= 5;
        }

        if (CharacterMovement.Skeleton)
        {
            currentHealth = 20;
            health = currentHealth;
        }
    }

    IEnumerator DamageCooldown()
    {
        isHitted = true;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isHitted = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyWeapon"))
        {
            EnemyBehaviours enemy = collision.gameObject.GetComponentInParent<EnemyBehaviours>();
            health -= enemy.EnemyHitDamage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyWeapon"))
        {
            EnemyBehaviours enemy = other.gameObject.GetComponentInParent<EnemyBehaviours>();
            health -= enemy.EnemyHitDamage();
        }
    }

}
