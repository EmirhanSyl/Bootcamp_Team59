using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationHolder : MonoBehaviour
{
    public GameObject[] woodResouces;
    public GameObject[] stoneResouces;
    public GameObject[] foodResouces;

    private void Awake()
    {
        woodResouces = GameObject.FindGameObjectsWithTag("WoodResource");
        stoneResouces = GameObject.FindGameObjectsWithTag("StoneResource");
        foodResouces = GameObject.FindGameObjectsWithTag("FoodResource");
    }
}
