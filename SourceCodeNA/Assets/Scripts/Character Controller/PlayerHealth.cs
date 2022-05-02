using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        health = healthOnInspector;
        currentHealth = health;

        animator = GetComponent<Animator>();
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

    void TransformToSkeleton()
    {

    }

    IEnumerator DamageCooldown()
    {
        isHitted = true;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isHitted = false;
    }
}
