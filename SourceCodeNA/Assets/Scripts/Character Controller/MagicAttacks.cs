using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI.MonsterBehavior;

public class MagicAttacks : MonoBehaviour
{
    public bool poisonMagicLock;
    public bool electricMagicLock;
    public bool healMagicLock;

    [SerializeField] private float magicCooldown = 10f;
    [SerializeField] private float particleDuration = 5f;

    [SerializeField] private ParticleSystem poisonMagic;
    [SerializeField] private ParticleSystem electricMagic;
    [SerializeField] private ParticleSystem healMagic;

    [SerializeField] private Transform targetIndicator;

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

        if (Input.GetKeyDown(KeyCode.Alpha1) && poisonMagicLock && magicCooldownTimer > magicCooldown && enemyDetector.CurrentTarget() != null)
        {
            poisonMagic.transform.position = targetIndicator.position;
            poisonMagic.gameObject.SetActive(true);
            poisonMagic.Play();
            particlePlaying = true;
            magicCooldownTimer = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && electricMagicLock && magicCooldownTimer > magicCooldown && enemyDetector.CurrentTarget() != null)
        {
            electricMagic.transform.position = new Vector3(targetIndicator.position.x, targetIndicator.position.y + 2f, targetIndicator.position.z);
            electricMagic.gameObject.SetActive(true);
            electricMagic.Play();
            particlePlaying = true;
            magicCooldownTimer = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && healMagicLock && magicCooldownTimer > magicCooldown)
        {
            healMagic.transform.position = transform.position;
            healMagic.gameObject.SetActive(true);
            healMagic.Play();
            particlePlaying = true;
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
