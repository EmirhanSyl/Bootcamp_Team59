using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class RegionManager : MonoBehaviour
{
    public int soilderCount;
    public Region regionStats;
    public LayerMask friendRegionsLayers;

    public int troopCount = 3;

    [SerializeField] private GameObject soilder;
    [SerializeField] private GameObject groupManager;
    [SerializeField] private Transform spawnPosition;

    [SerializeField] private InformationHolder informationHolder;

    private float troopSpawnCooldownTimer = 15;

    private int woodStat;
    private int stoneStat;
    private int foodStat;
    private int militaryPowerStat;
    private int soilderCapacity;

    private GameObject targetResource;
    private GameObject[] firstNeed;
    private GameObject[] secondNeed;
    private List<int> resourcesList = new List<int>(3);

    void Start()
    {
       woodStat = regionStats.Wood;
       stoneStat = regionStats.Stone;
       foodStat = regionStats.Food;
       militaryPowerStat = regionStats.MilitaryPower;

        soilderCount = militaryPowerStat * 5;        
    }

    
    void Update()
    {
        if (troopSpawnCooldownTimer > 0f)
        {
            troopSpawnCooldownTimer -= Time.deltaTime;
        }

        if (troopCount > 0 && troopSpawnCooldownTimer <= 0f)
        {
            CompareStats();
            troopSpawnCooldownTimer = 15f;
        }

        militaryPowerStat = soilderCount / 5;
    }

    void CompareStats()
    {
        resourcesList = new List<int>(3);
        resourcesList.Add(woodStat);
        resourcesList.Add(stoneStat);
        resourcesList.Add(foodStat);
        resourcesList.Sort();

        if (resourcesList[0] == woodStat)
        {
            firstNeed = informationHolder.woodResouces;
        }
        else if (resourcesList[0] == stoneStat)
        {
            firstNeed = informationHolder.stoneResouces;
        }
        else if (resourcesList[0] == foodStat)
        {
            firstNeed = informationHolder.foodResouces;
        }
        //else if (resourcesList[0] == militaryPowerStat)
        //{

        //}

        if (resourcesList[1] == woodStat)
        {
            secondNeed = informationHolder.woodResouces;
        }
        else if (resourcesList[1] == stoneStat)
        {
            secondNeed = informationHolder.stoneResouces;
        }
        else if (resourcesList[1] == foodStat)
        {
            secondNeed = informationHolder.foodResouces;
        }
        //else if (resourcesList[1] == militaryPowerStat)
        //{

        //}

        if (firstNeed != null)
        {
            SelectResoruce();
        }
    }

    void SelectResoruce()
    {
        float distanceToClosestResource = Mathf.Infinity;
        GameObject closestResource = null;

        foreach (var currentResource in firstNeed)
        {
            if (currentResource.GetComponent<ResourceInformations>().isExploiting && currentResource.GetComponent<ResourceInformations>().whichRegionOnResource == friendRegionsLayers)
            {
                continue;
            }
            float distanceToCurrentResource = (currentResource.transform.position - spawnPosition.position).sqrMagnitude;
            if (distanceToCurrentResource < distanceToClosestResource)
            {
                distanceToClosestResource = distanceToCurrentResource;
                closestResource = currentResource;
                targetResource = closestResource;
            }
        }

        if (targetResource != null)
        {
            SpawnTroop();
        }
    }

    void SpawnTroop()
    {
        if (troopCount <= 0 || targetResource == null)
        {
            return;
        }

        GameObject instantiatedGroupManager = Instantiate(groupManager, spawnPosition.position, Quaternion.identity);
        instantiatedGroupManager.transform.parent = transform;
        NPCGroupManager groupManagerScript = instantiatedGroupManager.GetComponent<NPCGroupManager>();
        groupManagerScript.groupRegionDropdown = regionStats.RegionSelection;
        groupManagerScript.enemyMask = regionStats.EnemyRegions;
        groupManagerScript.groupTargeResource = targetResource;
        groupManagerScript.regionManager = this;

        if (militaryPowerStat > 8 )
        {
            soilderCapacity = 8;
        }
        else if(militaryPowerStat > 6 && militaryPowerStat <= 8)
        {
            soilderCapacity = 6;
        }
        else if (militaryPowerStat > 4 && militaryPowerStat <= 6)
        {
            soilderCapacity = 4;
        }
        else if (militaryPowerStat > 2 && militaryPowerStat <= 4)
        {
            soilderCapacity = 3;
        }
        else
        {
            soilderCapacity = 1;
        }

        for (int i = 0; i < soilderCapacity; i++)
        {
            var instantiatedSoilder = Instantiate(soilder, new Vector3(spawnPosition.position.x + (i * 1.5f), spawnPosition.position.y, spawnPosition.position.z), Quaternion.identity);
            instantiatedSoilder.transform.parent = instantiatedGroupManager.transform;
            instantiatedSoilder.GetComponent<UnityEngine.AI.MonsterBehavior.EnemyBehaviours>().BackupAwake();
            if (i == soilderCapacity - 1)
            {
                groupManagerScript.BackupStart();
            }
        }

        if (!targetResource.GetComponent<ResourceInformations>().isExploiting)
        {
            targetResource.GetComponent<ResourceInformations>().SetInformations(friendRegionsLayers, soilderCapacity);
        }
        targetResource = null;
        troopCount -= 1;
    }
}
