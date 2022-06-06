using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI.MonsterBehavior;
using DG.Tweening;

public class PlayerHealth : MonoBehaviour
{
    public static float health;

    public static bool isHitted;
    public static bool dead;
    public bool deadIndicator;

    public float healthOnInspector = 100f;

    [SerializeField] private float noTakeDamageTime = 0.5f;
    [SerializeField] private float shakeLenght = 1f;
    [SerializeField] private float shakeStrenght = 3f;

    [SerializeField] private float healDuration = 3f;

    [SerializeField] private EnemyHealthBar healthBar;

    private float currentHealth;
    private float healingTimer;

    private bool godMode;

    private MagicAttacks magicAttacks;
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        magicAttacks = GetComponent<MagicAttacks>();

        health = healthOnInspector;
        currentHealth = health;
        //healthBar.SetMaxHealth(health);
    }

    
    void Update()
    {
        deadIndicator = dead;
        if (health != currentHealth && currentHealth > 0)
        {
            animator.SetTrigger("DamageTaken");
            currentHealth = health;
            Camera.main.transform.DOShakePosition(shakeLenght, shakeStrenght);
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
            animator = transform.GetChild(1).gameObject.GetComponent<Animator>();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            health -= 110;
        }

        healthBar.SetHealth(health);
    }
    public void DamageTakenAnimPlaying()
    {
        isHitted = true;
    }
    public void DamageTakenAnimStopped()
    {
        isHitted = false;
    }

    void Healing()
    {
        healingTimer += Time.deltaTime;

        if (healingTimer >= healDuration)
        {
            health += magicAttacks.currentHealAmount_Heal;
            if (health > 300)
            {
                health = 100;
            }
            healingTimer = 0f;
        }
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
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("HealMagic"))
        {
            Healing();
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
