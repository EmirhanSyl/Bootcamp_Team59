using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI.MonsterBehavior;

public class NPCGroupManager : MonoBehaviour
{
    public bool questDone;
    public EnemyBehaviours.Region groupRegionDropdown;

    public enum TroopType {AttackerTroop, GuardianTroop, DefenderTroop };
    public TroopType troopTypeDropdown;

    public bool troopCalledByPlayer;
    public bool troopHasQuest;
    public bool attackersOnCastleBorder;
    public bool troopCalledBack;

    public GameObject groupTargetResource;
    public GameObject castleLocation;
    public GameObject militaryBase;
    public GameObject storage;
    public GameObject attackerNPC;
    public RegionManager regionManager;
    public LayerMask enemyMask;

    public Collider[] enemyHitColliders;
    public EnemyBehaviours[] NPCGroup;

    private GameObject troopTarget;
    private GameObject player;

    void Start()
    {
        NPCGroup = new EnemyBehaviours[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            NPCGroup[i] = transform.GetChild(i).gameObject.GetComponent<EnemyBehaviours>();

            NPCGroup[i].regionDrowpdown = groupRegionDropdown;
            NPCGroup[i].controllingByGroupManager = true;
            if (groupTargetResource != null)
            {
                NPCGroup[i].protectedResource = groupTargetResource;
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

        switch (troopTypeDropdown)
        {
            case TroopType.AttackerTroop:
                AttackerTroopBehaviors();
                break;

            case TroopType.GuardianTroop:
                GuardianTroopBehaviors();
                break;

            case TroopType.DefenderTroop:
                DefenderTroopBehaviors();
                break;
        }

        if (troopCalledBack)
        {
            groupTargetResource = null;
            troopCalledByPlayer = false;
            troopHasQuest = false;
            //attackersOnCastleBorder = false;
        }
    }

    void AttackerTroopBehaviors()
    {
        if (troopCalledByPlayer)
        {
            troopTarget = player;
        }
        else
        {
            troopTarget = militaryBase;
        }
    }
    void GuardianTroopBehaviors()
    {
        if (groupTargetResource != null)
        {
            troopHasQuest = true;
        }
        else
        {
            troopHasQuest = false;
        }

        if (troopHasQuest)
        {
            if (questDone)
            {
                troopTarget = storage;
                groupTargetResource = null;
                return;
            }
            troopTarget = groupTargetResource;
            for (int i = 0; i < NPCGroup.Length; i++)
            {
                NPCGroup[i].protectedResource = groupTargetResource;
            }
        }
        //else
        //{
        //    troopTarget = militaryBase;
        //}
    }
    void DefenderTroopBehaviors()
    {
        if (attackersOnCastleBorder)
        {
            troopTarget = attackerNPC;
        }
        else
        {
            troopTarget = militaryBase;
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
            if (groupTargetResource != null)
            {
                NPCGroup[i].protectedResource = groupTargetResource;
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
