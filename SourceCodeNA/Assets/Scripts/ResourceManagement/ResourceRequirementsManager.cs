using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ResourcesCosts", menuName = "Requirements Order")]
public class ResourceRequirementsManager : ScriptableObject
{
    [Header("Magic Upgrades")]

    //PoisonMagic UpgradeCost
    //Level 0 to 1
    public int foodCost_PoisonMagic_Level1;
    public int soulCost_PoisonMagic_Level1;
    //Level 1 to 2
    public int foodCost_PoisonMagic_Level2;
    public int soulCost_PoisonMagic_Level2;
    //Level2 to 3
    public int foodCost_PoisonMagic_Level3;
    public int soulCost_PoisonMagic_Level3;

    [Space(10)]
    //ElectricMagic UpgradeCost
    //Level 0 to 1
    public int foodCost_ElectricMagic_Level1;
    public int soulCost_ElectricMagic_Level1;
    //Level 1 to 2
    public int foodCost_ElectricMagic_Level2;
    public int soulCost_ElectricMagic_Level2;
    //Level2 to 3
    public int foodCost_ElectricMagic_Level3;
    public int soulCost_ElectricMagic_Level3;

    [Space(10)]
    //HealMagic UpgradeCost
    //Level 0 to 1
    public int foodCost_HealMagic_Level1;
    public int soulCost_HealMagic_Level1;
    //Level 1 to 2
    public int foodCost_HealMagic_Level2;
    public int soulCost_HealMagic_Level2;
    //Level2 to 3
    public int foodCost_HealMagic_Level3;
    public int soulCost_HealMagic_Level3;
}
