using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class RegionManager : MonoBehaviour
{
    public Region regionStats;
    public bool olesineBool;

    [SerializeField] private GameObject soilder;
    [SerializeField] private GameObject groupManager;

    [SerializeField] private InformationHolder informationHolder;

    private int woodStat;
    private int stoneStat;
    private int foodStat;
    private int militaryPowerStat;

    private List<int> resourcesList = new List<int>(4);

    void Start()
    {
       woodStat = regionStats.Wood;
       stoneStat = regionStats.Stone;
       foodStat = regionStats.Food;
       militaryPowerStat = regionStats.MilitaryPower;

        resourcesList.Add(woodStat);
        resourcesList.Add(stoneStat);
        resourcesList.Add(foodStat);
        resourcesList.Add(militaryPowerStat);
        
    }

    
    void Update()
    {
        if (olesineBool)
        {
            CompareStats();
        }
    }

    void CompareStats()
    {
        resourcesList.Sort();

        if (resourcesList[0] == woodStat)
        {

        }
        else if (resourcesList[0] == stoneStat)
        {

        }
        else if (resourcesList[0] == foodStat)
        {

        }
        else if (resourcesList[0] == militaryPowerStat)
        {

        }
        
    }
}
