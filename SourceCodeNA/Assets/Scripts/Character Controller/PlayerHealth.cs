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
        if (health != currentHealth)
        {
            animator.SetTrigger("DamageTaken");
            currentHealth = health;
        }

        if (currentHealth <= 0)
        {
            animator.SetTrigger("Dead");
        }

        if (Input.GetMouseButtonDown(0))
        {
            health -= 5;
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
