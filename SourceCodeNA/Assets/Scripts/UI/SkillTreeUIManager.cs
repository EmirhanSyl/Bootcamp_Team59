using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class SkillTreeUIManager : MonoBehaviour
{
    public ResourceRequirementsManager upgradeCosts;

    public int level_PoisonMagic;
    public int level_ElectricMagic;
    public int level_HealMagic;

    [SerializeField] private GameObject SkillTreeSection;

    [Space(10)] [SerializeField] private GameObject poisonMagicPanel;
    [SerializeField] private GameObject icon_LockpoisonMagic;
    [SerializeField] private Image icon_Poison, circle_poison;
    [SerializeField] private TMP_Text text_PoisonLevel;

    [SerializeField] private TMP_Text text_PoisonLevelAtInfoTab;
    [SerializeField] private TMP_Text text_PoisonUpgrateLevelAtInfoTab;
    [SerializeField] private TMP_Text text_FoodCost_PoisonUpgrateLevelAtInfoTab;
    [SerializeField] private TMP_Text text_SoulCost_PoisonUpgrateLevelAtInfoTab;


    [Space(10)] [SerializeField] private GameObject electricMagicPanel;
    [SerializeField] private GameObject icon_LockElectricMagic;
    [SerializeField] private Image icon_Electric, circle_Electric;
    [SerializeField] private TMP_Text text_ElectricLevel;

    [SerializeField] private TMP_Text text_ElectricLevelAtInfoTab;
    [SerializeField] private TMP_Text text_ElectricUpgrateLevelAtInfoTab;
    [SerializeField] private TMP_Text text_FoodCost_ElectricUpgrateLevelAtInfoTab;
    [SerializeField] private TMP_Text text_SoulCost_ElectricUpgrateLevelAtInfoTab;


    [Space(10)] [SerializeField] private GameObject healMagicPanel;
    [SerializeField] private GameObject icon_LockHealMagic;
    [SerializeField] private Image icon_Heal, circle_Heal;
    [SerializeField] private TMP_Text text_HealLevel;

    [SerializeField] private TMP_Text text_HealLevelAtInfoTab;
    [SerializeField] private TMP_Text text_HealUpgrateLevelAtInfoTab;
    [SerializeField] private TMP_Text text_FoodCost_HealUpgrateLevelAtInfoTab;
    [SerializeField] private TMP_Text text_SoulCost_HealUpgrateLevelAtInfoTab;


    [SerializeField] private RegionManager regionManager;
    [SerializeField] private MagicAttacks magicAttacks;

    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip clip_Poison;
    [SerializeField] private VideoClip clip_Electric;
    [SerializeField] private VideoClip clip_Heal;

    private int used_FoodResource;
    private int used_SoulResource;

    private bool lock_PoisonMagic;
    private bool lock_ElectricMagic;
    private bool lock_HealMagic;

    private bool magicSkillsPanelStatus;
    private bool status_PoisonMagicInfoPanel;
    private bool status_ElectricMagicInfoPanel;
    private bool status_HealMagicInfoPanel;

    private bool upgradeable_PoisonMagic;
    private bool upgradeable_ElectricMagic;
    private bool upgradeable_HealMagic;

    private bool stopLevel_PoisonMagic;
    private bool stopLevel_ElectricMagic;

    void Start()
    {
        SkillTreeSection.SetActive(false);
        poisonMagicPanel.SetActive(false);
        electricMagicPanel.SetActive(false);
        healMagicPanel.SetActive(false);

        lock_PoisonMagic = true;
        icon_LockpoisonMagic.SetActive(false);
        text_PoisonLevel.text = "Level: " + level_PoisonMagic.ToString();
        icon_Poison.color = new Color(icon_Poison.color.r, icon_Poison.color.g, icon_Poison.color.b, 1);
        circle_poison.color = new Color(circle_poison.color.r, circle_poison.color.g, circle_poison.color.b, 1);
    }

    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            if (magicSkillsPanelStatus)
            {
                SkillTreeSection.SetActive(false);
                magicSkillsPanelStatus = false;
            }
            else
            {
                SkillTreeSection.SetActive(true);
                magicSkillsPanelStatus = true;
            }
        }

        if (level_PoisonMagic > 0 && !stopLevel_PoisonMagic)
        {
            lock_ElectricMagic = true;
            icon_LockElectricMagic.SetActive(false);
            icon_Electric.color = new Color(icon_Electric.color.r, icon_Electric.color.g, icon_Electric.color.b, 1);
            circle_Electric.color = new Color(circle_Electric.color.r, circle_Electric.color.g, circle_Electric.color.b, 1);
            text_ElectricLevel.text = "Level: " + level_ElectricMagic.ToString();
            stopLevel_PoisonMagic = true;
        }
        if (level_ElectricMagic > 0 && !stopLevel_ElectricMagic)
        {
            lock_HealMagic = true;
            icon_LockHealMagic.SetActive(false);
            icon_Heal.color = new Color(icon_Heal.color.r, icon_Heal.color.g, icon_Heal.color.b, 1);
            text_HealLevel.text = "Level: " + level_HealMagic.ToString();
            stopLevel_ElectricMagic = true;
        }

        if ((!status_ElectricMagicInfoPanel && !status_HealMagicInfoPanel && !status_PoisonMagicInfoPanel) || !magicSkillsPanelStatus)
        {
            videoPlayer.Stop();
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (status_PoisonMagicInfoPanel)
            {
                poisonMagicPanel.SetActive(false);
                status_PoisonMagicInfoPanel = false;
            }
            else if (status_ElectricMagicInfoPanel)
            {
                electricMagicPanel.SetActive(false);
                status_ElectricMagicInfoPanel = false;
            }
            else if (status_HealMagicInfoPanel)
            {
                healMagicPanel.SetActive(false);
                status_HealMagicInfoPanel = false;
            }
            else if (magicSkillsPanelStatus)
            {
                SkillTreeSection.SetActive(false);
                magicSkillsPanelStatus = false;
            }
        }
    }

    public void PoisonMagicButton()
    {
        if (!lock_PoisonMagic)
        {
            return;
        }
        text_PoisonLevelAtInfoTab.text = "Lvl: " + level_PoisonMagic;
        text_PoisonUpgrateLevelAtInfoTab.text = "Lvl: " + level_PoisonMagic + " -> " + (level_PoisonMagic+1).ToString();

        switch (level_PoisonMagic)
        {
            case 0:
                text_FoodCost_PoisonUpgrateLevelAtInfoTab.text = upgradeCosts.foodCost_PoisonMagic_Level1.ToString() + "/" + regionManager.foodCount;
                text_SoulCost_PoisonUpgrateLevelAtInfoTab.text = upgradeCosts.soulCost_PoisonMagic_Level1.ToString() + "/" + regionManager.soulCount;
                if (regionManager.soulCount < upgradeCosts.soulCost_PoisonMagic_Level1)
                {
                    text_SoulCost_PoisonUpgrateLevelAtInfoTab.color = Color.red;
                }
                else
                {
                    text_SoulCost_PoisonUpgrateLevelAtInfoTab.color = Color.black;
                }
                if(regionManager.foodCount < upgradeCosts.foodCost_PoisonMagic_Level1)
                {
                    text_FoodCost_PoisonUpgrateLevelAtInfoTab.color = Color.red;
                }
                else
                {
                    text_FoodCost_PoisonUpgrateLevelAtInfoTab.color = Color.black;
                }
                break;
            case 1:
                text_FoodCost_PoisonUpgrateLevelAtInfoTab.text = upgradeCosts.foodCost_PoisonMagic_Level2.ToString() + "/" + regionManager.foodCount;
                text_SoulCost_PoisonUpgrateLevelAtInfoTab.text = upgradeCosts.soulCost_PoisonMagic_Level2.ToString() + "/" + regionManager.soulCount;
                if (regionManager.soulCount < upgradeCosts.soulCost_PoisonMagic_Level2)
                {
                    text_SoulCost_PoisonUpgrateLevelAtInfoTab.color = Color.red;
                }
                else
                {
                    text_SoulCost_PoisonUpgrateLevelAtInfoTab.color = Color.black;
                }
                if (regionManager.foodCount < upgradeCosts.foodCost_PoisonMagic_Level2)
                {
                    text_FoodCost_PoisonUpgrateLevelAtInfoTab.color = Color.red;
                }
                else
                {
                    text_FoodCost_PoisonUpgrateLevelAtInfoTab.color = Color.black;
                }
                break;
            case 2:
                text_FoodCost_PoisonUpgrateLevelAtInfoTab.text = upgradeCosts.foodCost_PoisonMagic_Level3.ToString() + "/" + regionManager.foodCount;
                text_SoulCost_PoisonUpgrateLevelAtInfoTab.text = upgradeCosts.soulCost_PoisonMagic_Level3.ToString() + "/" + regionManager.soulCount;
                if (regionManager.soulCount < upgradeCosts.soulCost_PoisonMagic_Level3)
                {
                    text_SoulCost_PoisonUpgrateLevelAtInfoTab.color = Color.red;
                }
                else
                {
                    text_SoulCost_PoisonUpgrateLevelAtInfoTab.color = Color.black;
                }
                if (regionManager.foodCount < upgradeCosts.foodCost_PoisonMagic_Level3)
                {
                    text_FoodCost_PoisonUpgrateLevelAtInfoTab.color = Color.red;
                }
                else
                {
                    text_FoodCost_PoisonUpgrateLevelAtInfoTab.color = Color.black;
                }
                break;
        }
        

        status_PoisonMagicInfoPanel = true;
        poisonMagicPanel.SetActive(true);
        videoPlayer.clip = clip_Poison;
        videoPlayer.Play();
    }
    public void UpgradePoisonMagicButton()
    {
        if (level_PoisonMagic == 3)
        {
            return;
        }
        switch (level_PoisonMagic)
        {
            case 0:
                if (regionManager.soulCount >= upgradeCosts.soulCost_PoisonMagic_Level1 && regionManager.foodCount >= upgradeCosts.foodCost_PoisonMagic_Level1)
                {
                    upgradeable_PoisonMagic = true;
                }
                else
                {
                    upgradeable_PoisonMagic = false;
                }
                used_FoodResource = upgradeCosts.foodCost_PoisonMagic_Level1;
                used_SoulResource = upgradeCosts.soulCost_PoisonMagic_Level1;
                break;
            case 1:
                if (regionManager.soulCount >= upgradeCosts.soulCost_PoisonMagic_Level2 && regionManager.foodCount >= upgradeCosts.foodCost_PoisonMagic_Level2)
                {
                    upgradeable_PoisonMagic = true;
                }
                else
                {
                    upgradeable_PoisonMagic = false;
                }
                used_FoodResource = upgradeCosts.foodCost_PoisonMagic_Level2;
                used_SoulResource = upgradeCosts.soulCost_PoisonMagic_Level2;
                break;
            case 2:
                if (regionManager.soulCount >= upgradeCosts.soulCost_PoisonMagic_Level3 && regionManager.foodCount >= upgradeCosts.foodCost_PoisonMagic_Level3)
                {
                    upgradeable_PoisonMagic = true;
                }
                else
                {
                    upgradeable_PoisonMagic = false;
                }
                used_FoodResource = upgradeCosts.foodCost_PoisonMagic_Level3;
                used_SoulResource = upgradeCosts.soulCost_PoisonMagic_Level3;
                break;                
        }
        if (!upgradeable_PoisonMagic)
        {
            return;
        }

        level_PoisonMagic++;
        magicAttacks.poisonMagicLock = true;
        text_PoisonLevel.text = "Level: " + level_PoisonMagic.ToString();
        poisonMagicPanel.SetActive(false);
        status_PoisonMagicInfoPanel = false;
        regionManager.foodCount -= used_FoodResource;
        regionManager.soulCount -= used_SoulResource;
    }    


    public void ElectricMagicButton()
    {
        if (!lock_ElectricMagic)
        {
            return;
        }
        text_ElectricLevelAtInfoTab.text = "Lvl: " + level_ElectricMagic;
        text_ElectricUpgrateLevelAtInfoTab.text = "Lvl: " + level_ElectricMagic + " -> " + (level_ElectricMagic + 1).ToString();

        switch (level_ElectricMagic)
        {
            case 0:
                text_FoodCost_ElectricUpgrateLevelAtInfoTab.text = upgradeCosts.foodCost_ElectricMagic_Level1.ToString() + "/" + regionManager.foodCount;
                text_SoulCost_ElectricUpgrateLevelAtInfoTab.text = upgradeCosts.soulCost_ElectricMagic_Level1.ToString() + "/" + regionManager.soulCount;
                if (regionManager.soulCount < upgradeCosts.soulCost_ElectricMagic_Level1)
                {
                    text_SoulCost_ElectricUpgrateLevelAtInfoTab.color = Color.red;
                }
                else
                {
                    text_SoulCost_ElectricUpgrateLevelAtInfoTab.color = Color.black;
                }
                if (regionManager.foodCount < upgradeCosts.foodCost_ElectricMagic_Level1)
                {
                    text_FoodCost_ElectricUpgrateLevelAtInfoTab.color = Color.red;
                }
                else
                {
                    text_FoodCost_ElectricUpgrateLevelAtInfoTab.color = Color.black;
                }
                break;
            case 1:
                text_FoodCost_ElectricUpgrateLevelAtInfoTab.text = upgradeCosts.foodCost_ElectricMagic_Level2.ToString() + "/" + regionManager.foodCount;
                text_SoulCost_ElectricUpgrateLevelAtInfoTab.text = upgradeCosts.soulCost_ElectricMagic_Level2.ToString() + "/" + regionManager.soulCount;
                if (regionManager.soulCount < upgradeCosts.soulCost_ElectricMagic_Level2)
                {
                    text_SoulCost_ElectricUpgrateLevelAtInfoTab.color = Color.red;
                }
                else
                {
                    text_SoulCost_ElectricUpgrateLevelAtInfoTab.color = Color.black;
                }
                if (regionManager.foodCount < upgradeCosts.foodCost_ElectricMagic_Level2)
                {
                    text_FoodCost_ElectricUpgrateLevelAtInfoTab.color = Color.red;
                }
                else
                {
                    text_FoodCost_ElectricUpgrateLevelAtInfoTab.color = Color.black;
                }
                break;
            case 2:
                text_FoodCost_ElectricUpgrateLevelAtInfoTab.text = upgradeCosts.foodCost_ElectricMagic_Level3.ToString() + "/" + regionManager.foodCount;
                text_SoulCost_ElectricUpgrateLevelAtInfoTab.text = upgradeCosts.soulCost_ElectricMagic_Level3.ToString() + "/" + regionManager.soulCount;
                if (regionManager.soulCount < upgradeCosts.soulCost_ElectricMagic_Level3)
                {
                    text_SoulCost_ElectricUpgrateLevelAtInfoTab.color = Color.red;
                }
                else
                {
                    text_SoulCost_ElectricUpgrateLevelAtInfoTab.color = Color.black;
                }
                if (regionManager.foodCount < upgradeCosts.foodCost_ElectricMagic_Level3)
                {
                    text_SoulCost_ElectricUpgrateLevelAtInfoTab.color = Color.red;
                }
                else
                {
                    text_SoulCost_ElectricUpgrateLevelAtInfoTab.color = Color.black;
                }
                break;
        }

        status_ElectricMagicInfoPanel = true;
        electricMagicPanel.SetActive(true);
        videoPlayer.clip = clip_Electric;
        videoPlayer.Play();
    }
    public void UpgradeElectricMagicButton()
    {
        if (level_ElectricMagic == 3)
        {
            return;
        }
        switch (level_ElectricMagic)
        {
            case 0:
                if (regionManager.soulCount >= upgradeCosts.soulCost_ElectricMagic_Level1 && regionManager.foodCount >= upgradeCosts.foodCost_ElectricMagic_Level1)
                {
                    upgradeable_ElectricMagic = true;
                }
                else
                {
                    upgradeable_ElectricMagic = false;
                }
                used_FoodResource = upgradeCosts.foodCost_ElectricMagic_Level1;
                used_SoulResource = upgradeCosts.soulCost_ElectricMagic_Level1;
                break;
            case 1:
                if (regionManager.soulCount >= upgradeCosts.soulCost_ElectricMagic_Level2 && regionManager.foodCount >= upgradeCosts.foodCost_ElectricMagic_Level2)
                {
                    upgradeable_ElectricMagic = true;
                }
                else
                {
                    upgradeable_ElectricMagic = false;
                }
                used_FoodResource = upgradeCosts.foodCost_ElectricMagic_Level2;
                used_SoulResource = upgradeCosts.soulCost_ElectricMagic_Level2;
                break;
            case 2:
                if (regionManager.soulCount >= upgradeCosts.soulCost_ElectricMagic_Level3 && regionManager.foodCount >= upgradeCosts.foodCost_ElectricMagic_Level3)
                {
                    upgradeable_ElectricMagic = true;
                }
                else
                {
                    upgradeable_ElectricMagic = false;
                }
                used_FoodResource = upgradeCosts.foodCost_ElectricMagic_Level3;
                used_SoulResource = upgradeCosts.soulCost_ElectricMagic_Level3;
                break;
        }
        if (!upgradeable_ElectricMagic)
        {
            return;
        }

        level_ElectricMagic++;
        magicAttacks.electricMagicLock = true;
        text_ElectricLevel.text = "Level: " + level_ElectricMagic.ToString();
        electricMagicPanel.SetActive(false);
        status_ElectricMagicInfoPanel = false;
        regionManager.foodCount -= used_FoodResource;
        regionManager.soulCount -= used_SoulResource;
    }


    public void HealMagicButton()
    {
        if (!lock_HealMagic)
        {
            return;
        }
        text_HealLevelAtInfoTab.text = "Lvl: " + level_HealMagic;
        text_HealUpgrateLevelAtInfoTab.text = "Lvl: " + level_HealMagic + " -> " + (level_HealMagic + 1).ToString();

        switch (level_HealMagic)
        {
            case 0:
                text_FoodCost_HealUpgrateLevelAtInfoTab.text = upgradeCosts.foodCost_HealMagic_Level1.ToString() + "/" + regionManager.foodCount;
                text_SoulCost_HealUpgrateLevelAtInfoTab.text = upgradeCosts.soulCost_HealMagic_Level1.ToString() + "/" + regionManager.soulCount;
                if (regionManager.soulCount < upgradeCosts.soulCost_HealMagic_Level1)
                {
                    text_SoulCost_HealUpgrateLevelAtInfoTab.color = Color.red;
                }
                else
                {
                    text_SoulCost_HealUpgrateLevelAtInfoTab.color = Color.black;
                }
                if (regionManager.foodCount < upgradeCosts.foodCost_HealMagic_Level1)
                {
                    text_FoodCost_HealUpgrateLevelAtInfoTab.color = Color.red;
                }
                else
                {
                    text_FoodCost_HealUpgrateLevelAtInfoTab.color = Color.black;
                }
                break;
            case 1:
                text_FoodCost_HealUpgrateLevelAtInfoTab.text = upgradeCosts.foodCost_HealMagic_Level2.ToString() + "/" + regionManager.foodCount;
                text_SoulCost_HealUpgrateLevelAtInfoTab.text = upgradeCosts.soulCost_HealMagic_Level2.ToString() + "/" + regionManager.soulCount;
                if (regionManager.soulCount < upgradeCosts.soulCost_HealMagic_Level2)
                {
                    text_SoulCost_HealUpgrateLevelAtInfoTab.color = Color.red;
                }
                else
                {
                    text_SoulCost_HealUpgrateLevelAtInfoTab.color = Color.black;
                }
                if (regionManager.foodCount < upgradeCosts.foodCost_HealMagic_Level2)
                {
                    text_FoodCost_HealUpgrateLevelAtInfoTab.color = Color.red;
                }
                else
                {
                    text_FoodCost_HealUpgrateLevelAtInfoTab.color = Color.black;
                }
                break;
            case 2:
                text_FoodCost_HealUpgrateLevelAtInfoTab.text = upgradeCosts.foodCost_HealMagic_Level3.ToString() + "/" + regionManager.foodCount;
                text_SoulCost_HealUpgrateLevelAtInfoTab.text = upgradeCosts.soulCost_HealMagic_Level3.ToString() + "/" + regionManager.soulCount;
                if (regionManager.soulCount < upgradeCosts.soulCost_HealMagic_Level3)
                {
                    text_SoulCost_HealUpgrateLevelAtInfoTab.color = Color.red;
                }
                else
                {
                    text_SoulCost_HealUpgrateLevelAtInfoTab.color = Color.black;
                }
                if (regionManager.foodCount < upgradeCosts.foodCost_HealMagic_Level3)
                {
                    text_FoodCost_HealUpgrateLevelAtInfoTab.color = Color.red;
                }
                else
                {
                    text_FoodCost_HealUpgrateLevelAtInfoTab.color = Color.black;
                }
                break;
        }


        status_HealMagicInfoPanel = true;
        healMagicPanel.SetActive(true);
        videoPlayer.clip = clip_Heal;
        videoPlayer.Play();
    }
    public void UpgradeHealMagicButton()
    {
        if (level_HealMagic == 3)
        {
            return;
        }
        switch (level_HealMagic)
        {
            case 0:
                if (regionManager.soulCount >= upgradeCosts.soulCost_HealMagic_Level1 && regionManager.foodCount >= upgradeCosts.foodCost_HealMagic_Level1)
                {
                    upgradeable_HealMagic = true;
                }
                else
                {
                    upgradeable_HealMagic = false;
                }
                used_FoodResource = upgradeCosts.foodCost_HealMagic_Level1;
                used_SoulResource = upgradeCosts.soulCost_HealMagic_Level1;
                break;
            case 1:
                if (regionManager.soulCount >= upgradeCosts.soulCost_HealMagic_Level2 && regionManager.foodCount >= upgradeCosts.foodCost_HealMagic_Level2)
                {
                    upgradeable_HealMagic = true;
                }
                else
                {
                    upgradeable_HealMagic = false;
                }
                used_FoodResource = upgradeCosts.foodCost_HealMagic_Level2;
                used_SoulResource = upgradeCosts.soulCost_HealMagic_Level2;
                break;
            case 2:
                if (regionManager.soulCount >= upgradeCosts.soulCost_HealMagic_Level3 && regionManager.foodCount >= upgradeCosts.foodCost_HealMagic_Level3)
                {
                    upgradeable_HealMagic = true;
                }
                else
                {
                    upgradeable_HealMagic = false;
                }
                used_FoodResource = upgradeCosts.foodCost_HealMagic_Level3;
                used_SoulResource = upgradeCosts.soulCost_HealMagic_Level3;
                break;
        }
        if (!upgradeable_HealMagic)
        {
            return;
        }

        level_HealMagic++;
        magicAttacks.healMagicLock = true;
        text_HealLevel.text = "Level: " + level_HealMagic.ToString();
        healMagicPanel.SetActive(false);
        status_HealMagicInfoPanel = false;
        regionManager.foodCount -= used_FoodResource;
        regionManager.soulCount -= used_SoulResource;
    }



}
