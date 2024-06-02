using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
{
    public static UpgradeController Instance;

    [Header("Upgrade Costs")]
    [SerializeField]
    private int upgradeArcherCost;
    [SerializeField]
    private int upgradeWallCost;
    [SerializeField]
    private int upgradeWizardCost;
    [SerializeField]
    private int upgradeCrossbowManCost;
    [SerializeField]
    private int upgradeFairyCost;

    [Header("Upgrade Improvements")]
    [SerializeField]
    private float upgradedArcherAttackTime;
    [SerializeField]
    private int upgradedWallMaxHPIncrease;
    [SerializeField]
    private float upgradedWizardExtraPolymorphTime;
    [SerializeField]
    private int upgradedCrossbowManExtraAttack;
    [SerializeField]
    private float upgradedFairySpeedIncrease;

    [Header("Upgrade Sprites")]
    [SerializeField]
    private Sprite upgradedArcherSprite;
    [SerializeField]
    private Sprite upgradedWallSprite;
    [SerializeField]
    private Sprite upgradedWizardSprite;
    [SerializeField]
    private Sprite upgradedCrossbowManSprite;
    [SerializeField]
    private Sprite upgradedFairySprite;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI upgradeArcherCostText;
    [SerializeField]
    private GameObject upgradeArcherButton;
    [SerializeField]
    private Image placeArcherImage;

    [SerializeField]
    private TextMeshProUGUI upgradeWallCostText;
    [SerializeField]
    private GameObject upgradeWallButton;
    [SerializeField]
    private Image placeWallImage;

    [SerializeField]
    private TextMeshProUGUI upgradeWizardCostText;
    [SerializeField]
    private GameObject upgradeWizardButton;
    [SerializeField]
    private Image placeWizardImage;

    [SerializeField]
    private TextMeshProUGUI upgradeCrossbowManCostText;
    [SerializeField]
    private GameObject upgradeCrossbowManButton;
    [SerializeField]
    private Image placeCrossbowManImage;

    [SerializeField]
    private TextMeshProUGUI upgradeFairyCostText;
    [SerializeField]
    private GameObject upgradeFairyButton;
    [SerializeField]
    private Image placeFairyImage;

    private bool upgradedArcher = false;
    private bool upgradedWall = false;
    private bool upgradedWizard = false;
    private bool upgradedCrossbowMan = false;
    private bool upgradedFairy = false;

    [SerializeField]
    private float towerRangeBonus = 3f;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // If an instance already exists and it's not this one, destroy this instance
            Destroy(this.gameObject);
        }
        else
        {
            // Set this instance as the singleton instance if it's the first one
            Instance = this;
        }
    }

    private void Start()
    {
        upgradeArcherCostText.text = upgradeArcherCost.ToString();
        upgradeWizardCostText.text = upgradeWizardCost.ToString();
        upgradeWallCostText.text = upgradeWallCost.ToString();
        upgradeCrossbowManCostText.text = upgradeCrossbowManCost.ToString();
        upgradeFairyCostText.text = upgradeFairyCost.ToString();
    }

    public void UpgradeArchers()
    {
        if (ResourceController.Instance.CanSpendCoins(upgradeArcherCost) == true)
        {
            upgradedArcher = true;
            ResourceController.Instance.SpendCoins(upgradeArcherCost);
            UpdateAllCurrentArchers();
            upgradeArcherButton.SetActive(false);
            placeArcherImage.sprite = upgradedArcherSprite;
        }
    }

    public void UpgradeWalls()
    {
        if (ResourceController.Instance.CanSpendCoins(upgradeWallCost) == true)
        {
            upgradedWall = true;
            ResourceController.Instance.SpendCoins(upgradeWallCost);
            UpdateAllCurrentWalls();
            upgradeWallButton.SetActive(false);
            placeWallImage.sprite = upgradedWallSprite;
        }
    }

    public void UpgradeWizards()
    {
        if (ResourceController.Instance.CanSpendCoins(upgradeWizardCost) == true)
        {
            upgradedWizard = true;
            ResourceController.Instance.SpendCoins(upgradeWizardCost);
            UpdateAllCurrentWizards();
            upgradeWizardButton.SetActive(false);
            placeWizardImage.sprite = upgradedWizardSprite;
        }
    }

    public void UpgradeCrossbowMen()
    {
        if (ResourceController.Instance.CanSpendCoins(upgradeCrossbowManCost) == true)
        {
            upgradedCrossbowMan = true;
            ResourceController.Instance.SpendCoins(upgradeCrossbowManCost);
            UpdateAllCurrentCrossbowMen();
            upgradeCrossbowManButton.SetActive(false);
            placeCrossbowManImage.sprite = upgradedCrossbowManSprite;
        }
    }

    public void UpgradeFairies()
    {
        if (ResourceController.Instance.CanSpendCoins(upgradeFairyCost) == true)
        {
            upgradedFairy = true;
            ResourceController.Instance.SpendCoins(upgradeFairyCost);
            UpdateAllCurrentFairies();
            upgradeFairyButton.SetActive(false);
            placeFairyImage.sprite = upgradedFairySprite;
        }
    }

    private void UpdateAllCurrentArchers()
    {
        List<GameObject> playerObjects = ObjectPlacer.Instance.GetPlayerAttackables();
        foreach (GameObject playerObject in playerObjects)
        {
            ArcherAttacker archer = playerObject.GetComponent<ArcherAttacker>();
            if (archer != null)
            {
                UpgradeArcher(archer);
            }
        }
        foreach (GameObject towerUnit in ObjectPlacer.Instance.GetToweredUnits())
        {
            ArcherAttacker archer = towerUnit.GetComponent<ArcherAttacker>();
            if (archer != null)
            {
                UpgradeArcher(archer);
            }
        }
    }

    private void UpdateAllCurrentFairies()
    {
        List<GameObject> fairies = ObjectPlacer.Instance.GetFairies();
        foreach (GameObject fairyObj in fairies)
        {
            Fairy fairy = fairyObj.GetComponent<Fairy>();
            if (fairy != null)
            {
                UpgradeFairy(fairy);
            }
        }
    }

    private void UpdateAllCurrentCrossbowMen()
    {
        List<GameObject> playerObjects = ObjectPlacer.Instance.GetPlayerAttackables();
        foreach (GameObject playerObject in playerObjects)
        {
            CrossbowAttacker crossbowMan = playerObject.GetComponent<CrossbowAttacker>();
            if (crossbowMan != null)
            {
                UpgradeCrossbowMan(crossbowMan);
            }
        }
        foreach (GameObject towerUnit in ObjectPlacer.Instance.GetToweredUnits())
        {
            CrossbowAttacker crossbowMan = towerUnit.GetComponent<CrossbowAttacker>();
            if (crossbowMan != null)
            {
                UpgradeCrossbowMan(crossbowMan);
            }
        }
    }

    private void UpdateAllCurrentWizards()
    {
        List<GameObject> playerObjects = ObjectPlacer.Instance.GetPlayerAttackables();
        foreach (GameObject playerObject in playerObjects)
        {
            WizardAttacker wizard = playerObject.GetComponent<WizardAttacker>();
            if (wizard != null)
            {
                UpgradeWizard(wizard);
            }
        }
        foreach (GameObject towerUnit in ObjectPlacer.Instance.GetToweredUnits())
        {
            WizardAttacker wizard = towerUnit.GetComponent<WizardAttacker>();
            if (wizard != null)
            {
                UpgradeWizard(wizard);
            }
        }
    }

    private void UpdateAllCurrentWalls()
    {
        List<GameObject> playerObjects = ObjectPlacer.Instance.GetPlayerAttackables();
        foreach (GameObject playerObject in playerObjects)
        {
            Wall wall = playerObject.GetComponent<Wall>();
            if (wall != null)
            {
                UpgradeWall(wall);
            }
        }
        foreach (GameObject towerUnit in ObjectPlacer.Instance.GetToweredUnits())
        {
            Wall wall = towerUnit.GetComponent<Wall>();
            if (wall != null)
            {
                UpgradeWall(wall);
            }
        }
    }

    public void UpgradeWall(Wall wall)
    {
        wall.SetStoneWall();
        wall.CheckIfNeedToUpdateWallSprite();
        EntityStatus status = wall.GetComponent<EntityStatus>();
        if (status != null)
        {
            status.AddHP(upgradedWallMaxHPIncrease);
        }
    }

    public void UpgradeArcher(ArcherAttacker archer)
    {
        archer.SetUpgradedAttackTime(upgradedArcherAttackTime);
        SpriteRenderer spr = archer.GetComponent<SpriteRenderer>();
        if (spr != null)
        {
            spr.sprite = upgradedArcherSprite;
        }
    }

    public void UpgradeFairy(Fairy fairy)
    {
        fairy.SetUpgradedSpeed(upgradedFairySpeedIncrease);
        SpriteRenderer spr = fairy.GetComponent<SpriteRenderer>();
        if (spr != null)
        {
            spr.sprite = upgradedFairySprite;
        }
    }

    public void UpgradeCrossbowMan(CrossbowAttacker crossbowMan)
    {
        crossbowMan.SetUpgradedAttackDamage(upgradedCrossbowManExtraAttack);
        SpriteRenderer spr = crossbowMan.GetComponent<SpriteRenderer>();
        if (spr != null)
        {
            spr.sprite = upgradedCrossbowManSprite;
        }
    }

    public void UpgradeWizard(WizardAttacker wizard)
    {
        wizard.SetUpgradedPolymorphTime(upgradedWizardExtraPolymorphTime);
        SpriteRenderer spr = wizard.GetComponent<SpriteRenderer>();
        if (spr != null)
        {
            spr.sprite = upgradedWizardSprite;
        }
    }


    public bool HasUpgradedArcher()
    {
        return upgradedArcher;
    }

    public bool HasUpgradedWalls()
    {
        return upgradedWall;
    }

    public bool HasUpgradedWizard()
    {
        return upgradedWizard;
    }

    public bool HasUpgradedCrossbowMan()
    {
        return upgradedCrossbowMan;
    }

    public bool HasUpgradedFairy()
    {
        return upgradedFairy;
    }

    public float GetTowerRangeBonus()
    {
        return towerRangeBonus;
    }
}
