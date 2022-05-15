using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI.MonsterBehavior;

public class NPCGroupManager : MonoBehaviour
{    
    public EnemyBehaviours.Region groupRegionDropdown;

    public GameObject groupTargeResource;
    public RegionManager regionManager;
    public LayerMask enemyMask;

    public Collider[] enemyHitColliders;
    private EnemyBehaviours[] NPCGroup;

    private GameObject player;

    void Start()
    {
        NPCGroup = new EnemyBehaviours[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            NPCGroup[i] = transform.GetChild(i).gameObject.GetComponent<EnemyBehaviours>();

            NPCGroup[i].regionDrowpdown = groupRegionDropdown;
            NPCGroup[i].controllingByGroupManager = true;
            if (groupTargeResource != null)
            {
                NPCGroup[i].protectedResource = groupTargeResource;
            }

            if (NPCGroup[i].regionDrowpdown == EnemyBehaviours.Region.Forest)
            {
                NPCGroup[i].friendWithPlayer = true;
            }

            switch (NPCGroup[i].regionDrowpdown)
            {
                case EnemyBehaviours.Region.Desert:
                    NPCGroup[i].gameObject.transform.GetChild(2).gameObject.layer = 8;
                    break;
                case EnemyBehaviours.Region.Forest:
                    NPCGroup[i].gameObject.transform.GetChild(2).gameObject.layer = 9;
                    break;
                case EnemyBehaviours.Region.Ice:
                    NPCGroup[i].gameObject.transform.GetChild(2).gameObject.layer = 10;
                    break;
            }
        }

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (transform.childCount == 0)
        {
            regionManager.troopCount++;
            Destroy(gameObject);
        }
    }

    public void SomebodyDied()
    {
        regionManager.soilderCount--;
    }

    public void BackupStart()
    {
        NPCGroup = new EnemyBehaviours[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            NPCGroup[i] = transform.GetChild(i).gameObject.GetComponent<EnemyBehaviours>();

            NPCGroup[i].regionDrowpdown = groupRegionDropdown;
            NPCGroup[i].controllingByGroupManager = true;
            if (groupTargeResource != null)
            {
                NPCGroup[i].protectedResource = groupTargeResource;
            }

            if (NPCGroup[i].regionDrowpdown == EnemyBehaviours.Region.Forest)
            {
                NPCGroup[i].friendWithPlayer = true;
            }

            switch (NPCGroup[i].regionDrowpdown)
            {
                case EnemyBehaviours.Region.Desert:
                    NPCGroup[i].gameObject.transform.GetChild(2).gameObject.layer = 8;
                    break;
                case EnemyBehaviours.Region.Forest:
                    NPCGroup[i].gameObject.transform.GetChild(2).gameObject.layer = 9;
                    break;
                case EnemyBehaviours.Region.Ice:
                    NPCGroup[i].gameObject.transform.GetChild(2).gameObject.layer = 10;
                    break;
            }
        }

        player = GameObject.FindGameObjectWithTag("Player");
    }
}
