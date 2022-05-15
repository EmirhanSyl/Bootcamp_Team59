using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI.MonsterBehavior;

[CreateAssetMenu(fileName = "New Ragion", menuName = "Region")]
public class Region : ScriptableObject
{
    public new string name;
    public EnemyBehaviours.Region RegionSelection;

    [Header("Resources")]
    [Range(0, 10)] public int Wood;
    [Range(0, 10)] public int Stone;
    [Range(0, 10)] public int Food;
    [Range(0, 10)] public int MilitaryPower;
}
