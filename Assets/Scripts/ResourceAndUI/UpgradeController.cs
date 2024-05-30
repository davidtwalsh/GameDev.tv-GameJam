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

    [Header("Upgrade Improvements")]
    [SerializeField]
    private float upgradedArcherAttackTime;
    [SerializeField]
    private int upgradedWallMaxHPIncrease;

    [Header("Upgrade Sprites")]
    [SerializeField]
    private Sprite upgradedArcherSprite;
    [SerializeField]
    private Sprite upgradedWallSprite;

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

    private bool upgradedArcher = false;
    private bool upgradedWall = false;


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
            archer.GetComponent<SpriteRenderer>().sprite = upgradedArcherSprite;
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
}
