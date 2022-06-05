using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.AI.MonsterBehavior;
using DG.Tweening;

public class SiegeMachines : MonoBehaviour
{
    [Header("Enemy States")]
    [SerializeField] private float rowHealth = 100f;
    [SerializeField] private float movementSpeed = 1.5f;
    [SerializeField] private float SprintSpeed = 3.5f;

    [Header("Attack Stats")]
    public float maxDistanceToTarget = 20f;
    [SerializeField] private float attackRange = 4f;
    [SerializeField] private float attackDuration = 0.6f;
    [SerializeField] private float minHitDamage = 5f;
    [SerializeField] private float maxHitDamage = 25f;

    public float distanceToTarget;

    public EnemyBehaviours.Region targetRegion;
    public EnemyBehaviours.Region mechineRegion;
    public RegionManager targetRegionScript;


    [SerializeField] private GameObject healthBar;

    [SerializeField] private ParticleSystem woodenParticles;
    [SerializeField] private ParticleSystem smokeParticles;

    [SerializeField] private LayerMask enemyBuildingLayers;

    private float attackTimer;

    private bool isDestroyed;
    private bool isAttacking;
    private bool friendWithPlayer;

    private string enemyBuildingTag;

    private GameObject targetObject;
    private GameObject temporaryTarget;

    private NPCGroupManager groupManager;
    private CharacterMovement characterMovement;
    private WeaponController weaponController;
    private EnemyHealthBar healthBarScript;
    private Animator animator;
    private NavMeshAgent agent;

    public Collider[] cols;

    public bool _calledByPlayer;
    void Start()
    {
        characterMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
        groupManager = GetComponentInParent<NPCGroupManager>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        healthBarScript = healthBar.GetComponent<EnemyHealthBar>();
        healthBarScript.SetMaxHealth(rowHealth);
        switch (targetRegion)
        {
            case EnemyBehaviours.Region.Desert:
                targetRegionScript = GameObject.FindGameObjectWithTag("DesertManager").GetComponent<RegionManager>();
                enemyBuildingTag = "DesertBuilding";
                break;
            case EnemyBehaviours.Region.Forest:
                targetRegionScript = GameObject.FindGameObjectWithTag("ForestManager").GetComponent<RegionManager>();
                enemyBuildingTag = "ForestBuilding";
                break;
            case EnemyBehaviours.Region.Ice:
                targetRegionScript = GameObject.FindGameObjectWithTag("IceManager").GetComponent<RegionManager>();
                enemyBuildingTag = "IceBuilding";
                break;
        }

        if (mechineRegion == EnemyBehaviours.Region.Forest)
        {
            friendWithPlayer = true;
        }
    }

    
    void Update()
    {
        healthBarScript.SetHealth(rowHealth);

        if (rowHealth <= 0)
        {
            rowHealth = 0;
            isDestroyed = true;
        }
        if (isDestroyed)
        {
            targetObject = null;
            return;
        }  

        if (Physics.CheckSphere(gameObject.transform.position, maxDistanceToTarget, enemyBuildingLayers))
        {
            float distanceToClosestEnemy = Mathf.Infinity;
            GameObject closestEnemy = null;
            Collider[] allEnemies = Physics.OverlapSphere(gameObject.transform.position, maxDistanceToTarget, enemyBuildingLayers);
            cols = allEnemies;

            foreach (var currentEnemyColl in allEnemies)
            {
                if ((currentEnemyColl.GetComponentInParent<BuildingMnager>() != null && currentEnemyColl.GetComponentInParent<BuildingMnager>().isDestroyed) || (!currentEnemyColl.gameObject.transform.parent.gameObject.CompareTag("DesertBuilding")))
                {
                    Debug.Log("Target Removed");
                    targetObject = null;
                    return;
                }
                float distanceToCurrentEnemy = (currentEnemyColl.gameObject.transform.position - transform.position).sqrMagnitude;
                if (distanceToCurrentEnemy < distanceToClosestEnemy)
                {
                    distanceToClosestEnemy = distanceToCurrentEnemy;
                    closestEnemy = currentEnemyColl.gameObject.transform.parent.gameObject;
                    targetObject = closestEnemy;
                    Debug.Log("TARGEEEET");
                }
            }
        }
        else
        {
            if (temporaryTarget != null)
            {

                if ((transform.position - temporaryTarget.transform.position).magnitude > 10f)
                {
                    agent.SetDestination(temporaryTarget.transform.position);
                }
                else
                {
                    agent.SetDestination(transform.position);
                }
                return;
            }

            if (_calledByPlayer)
            {
                temporaryTarget = GameObject.FindGameObjectWithTag("Player");
            }            
        }

        Vector3 targetVector = targetObject.transform.parent.transform.position - (targetObject.transform.forward * 4f);
        transform.DOLookAt(new Vector3(targetObject.transform.position.x, transform.position.y, targetObject.transform.position.z), 2f);
        distanceToTarget = (targetVector - transform.position).sqrMagnitude;

        if (distanceToTarget < 3f)
        {
            agent.SetDestination(transform.position);
            Attack();
        }
        else
        {
            agent.SetDestination(targetVector);
        }


    }

    void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDuration)
        {
            animator.SetTrigger("Attack");
            if (attackTimer >= attackDuration + 0.5f && !isAttacking)
            {               
                attackTimer = 0f;
                smokeParticles.Stop();
            }
        }
    }

    public void RamAttackAnimPlaying()
    {
        isAttacking = true;
    }

    public void RamHitEvent()
    {
        targetObject.GetComponent<BuildingMnager>().buildingHealth -= Random.Range(minHitDamage, maxHitDamage);
        woodenParticles.Play();
        smokeParticles.Play();
    }

    public void RamAttackAnimStoped()
    {
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("PlayerWeapon") && characterMovement.isAttack && !friendWithPlayer)
        {
            rowHealth -= characterMovement.WeaponDamage();
            ManaManager.manaAmount += characterMovement.WeaponDamage();

            if (other.gameObject.GetComponent<WeaponController>() != null)
            {
                weaponController = other.gameObject.GetComponent<WeaponController>();
                weaponController.beAbleToAttack = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        if (targetObject != null)
        {
            Gizmos.DrawSphere(targetObject.transform.parent.transform.position - (targetObject.transform.forward * 4f), 1f);
        }
    }
}
