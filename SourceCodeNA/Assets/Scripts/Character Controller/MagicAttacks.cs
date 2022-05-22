using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI.MonsterBehavior;

public class MagicAttacks : MonoBehaviour
{
    [HideInInspector]public bool poisonMagicLock;
    [HideInInspector]public bool electricMagicLock;
    [HideInInspector]public bool healMagicLock;

    [HideInInspector]public bool magicLevelChanged;

    [HideInInspector]public int poisonLevel;
    [HideInInspector]public int electricLevel;
    [HideInInspector]public int healLevel;

    [SerializeField] private float magicCooldown = 10f;
    [SerializeField] private float particleDuration = 5f;

    [SerializeField] private ParticleSystem poisonMagic;
    [SerializeField] private ParticleSystem electricMagic;
    [SerializeField] private ParticleSystem healMagic;

    [SerializeField] private Transform targetIndicator;

    [Header("Poison Stats")]
    [Space(20)] [SerializeField] private Vector3 scaleVector_PoisonMagic_Level1;
    [SerializeField] private Vector3 scaleVector_PoisonMagic_Level2;
    [SerializeField] private Vector3 scaleVector_PoisonMagic_Level3;
    [Space(5)] public float damageAmount_PoisonMagic_Level1;
    public float damageAmount_PoisonMagic_Level2;
    public float damageAmount_PoisonMagic_Level3;
    [Space(5)] [SerializeField] private float duration_PoisonMagic_Level1;
    [SerializeField] private float duration_PoisonMagic_Level2;
    [SerializeField] private float duration_PoisonMagic_Level3;
    [HideInInspector] public float currentDamageAmount_Poison;
    private float poisonDuration;

    [Header("Electric Stats")]
    [Space(20)] [SerializeField] private Vector3 scaleVector_ElectricMagic_Level1;
    [SerializeField] private Vector3 scaleVector_ElectricMagic_Level2;
    [SerializeField] private Vector3 scaleVector_ElectricMagic_Level3;
    [Space(5)] public float damageAmount_ElectricMagic_Level1;
    public float damageAmount_ElectricMagic_Level2;
    public float damageAmount_ElectricMagic_Level3;
    [Space(5)] [SerializeField] private float duration_ElectricMagic_Level1;
    [SerializeField] private float duration_ElectricMagic_Level2;
    [SerializeField] private float duration_ElectricMagic_Level3;
    [HideInInspector] public float currentDamageAmount_Electric;
    private float electricDuration;

    [Header("Heal Stats")]
    [Space(20)] [SerializeField] private Vector3 scaleVector_HealMagic_Level1;
    [SerializeField] private Vector3 scaleVector_HealMagic_Level2;
    [SerializeField] private Vector3 scaleVector_HealMagic_Level3;
    [Space(5)] public float healAmount_HealMagic_Level1;
    public float healAmount_HealMagic_Level2;
    public float healAmount_HealMagic_Level3;
    [Space(5)] [SerializeField] private float duration_HealMagic_Level1;
    [SerializeField] private float duration_HealMagic_Level2;
    [SerializeField] private float duration_HealMagic_Level3;
    [HideInInspector] public float currentHealAmount_Heal;
    private float healDuration;

    private float magicCooldownTimer;

    private bool particlePlaying;

    private EnemyDetector enemyDetector;

    void Start()
    {
        enemyDetector = GetComponentInChildren<EnemyDetector>();

        poisonMagic.Stop();
        electricMagic.Stop();
        healMagic.Stop();
    }

   
    void Update()
    {
        magicCooldownTimer += Time.deltaTime;

        if (magicLevelChanged)
        {
            switch (poisonLevel)
            {
                case 1:
                    poisonMagic.gameObject.transform.localScale = scaleVector_PoisonMagic_Level1;
                    currentDamageAmount_Poison = damageAmount_PoisonMagic_Level1;
                    poisonDuration = duration_PoisonMagic_Level1;
                    break;
                case 2:
                    poisonMagic.gameObject.transform.localScale = scaleVector_PoisonMagic_Level2;
                    currentDamageAmount_Poison = damageAmount_PoisonMagic_Level2;
                    poisonDuration = duration_PoisonMagic_Level2;
                    break;
                case 3:
                    poisonMagic.gameObject.transform.localScale = scaleVector_PoisonMagic_Level3;
                    currentDamageAmount_Poison = damageAmount_PoisonMagic_Level3;
                    poisonDuration = duration_PoisonMagic_Level3;
                    break;
            }
            switch (electricLevel)
            {
                case 1:
                    electricMagic.gameObject.transform.localScale = scaleVector_ElectricMagic_Level1;
                    currentDamageAmount_Electric = damageAmount_ElectricMagic_Level1;
                    electricDuration = duration_ElectricMagic_Level1;
                    break;
                case 2:
                    electricMagic.gameObject.transform.localScale = scaleVector_ElectricMagic_Level2;
                    currentDamageAmount_Electric = damageAmount_ElectricMagic_Level2;
                    electricDuration = duration_ElectricMagic_Level2;
                    break;
                case 3:
                    electricMagic.gameObject.transform.localScale = scaleVector_ElectricMagic_Level3;
                    currentDamageAmount_Electric = damageAmount_ElectricMagic_Level3;
                    electricDuration = duration_ElectricMagic_Level3;
                    break;
            }
            switch (healLevel)
            {
                case 1:
                    healMagic.gameObject.transform.localScale = scaleVector_HealMagic_Level1;
                    currentHealAmount_Heal = healAmount_HealMagic_Level1;
                    healDuration = duration_HealMagic_Level1;
                    break;
                case 2:
                    healMagic.gameObject.transform.localScale = scaleVector_HealMagic_Level2;
                    currentHealAmount_Heal = healAmount_HealMagic_Level2;
                    healDuration = duration_HealMagic_Level2;
                    break;
                case 3:
                    healMagic.gameObject.transform.localScale = scaleVector_HealMagic_Level3;
                    currentHealAmount_Heal = healAmount_HealMagic_Level3;
                    healDuration = duration_HealMagic_Level3;
                    break;
            }
            magicLevelChanged = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && poisonMagicLock && magicCooldownTimer > magicCooldown && enemyDetector.CurrentTarget() != null && ManaManager.readyToMagic)
        {
            poisonMagic.transform.position = targetIndicator.position;
            poisonMagic.gameObject.SetActive(true);
            poisonMagic.Play();
            particlePlaying = true;
            particleDuration = poisonDuration;

            ManaManager.manaAmount = 1f;
            ManaManager.readyToMagic = false;
            magicCooldownTimer = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && electricMagicLock && magicCooldownTimer > magicCooldown && enemyDetector.CurrentTarget() != null && ManaManager.readyToMagic)
        {
            electricMagic.transform.position = new Vector3(targetIndicator.position.x, targetIndicator.position.y + 2f, targetIndicator.position.z);
            electricMagic.gameObject.SetActive(true);
            electricMagic.Play();
            particlePlaying = true;
            particleDuration = electricDuration;

            ManaManager.manaAmount = 1f;
            ManaManager.readyToMagic = false;
            magicCooldownTimer = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && healMagicLock && magicCooldownTimer > magicCooldown && ManaManager.readyToMagic)
        {
            healMagic.transform.position = transform.position;
            healMagic.gameObject.SetActive(true);
            healMagic.Play();
            particlePlaying = true;
            particleDuration = healDuration;

            ManaManager.manaAmount = 1f;
            ManaManager.readyToMagic = false;
            magicCooldownTimer = 0;
        }

        if (particlePlaying)
        {
            particleDuration -= Time.deltaTime;
            if (particleDuration <= 0f)
            {
                poisonMagic.Stop();
                electricMagic.Stop();
                healMagic.Stop();
                if (particleDuration <= -1f)
                {
                    poisonMagic.gameObject.SetActive(false);
                    electricMagic.gameObject.SetActive(false);
                    healMagic.gameObject.SetActive(false);
                    particleDuration = 5f;
                    particlePlaying = false;
                }
            }
        }
    }
}
