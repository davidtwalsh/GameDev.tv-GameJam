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

    [Header("Upgrade Improvements")]
    [SerializeField]
    private float upgradedArcherAttackTime;

    [Header("Upgrade Sprites")]
    [SerializeField]
    private Sprite upgradedArcherSprite;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI upgradeArcherCostText;
    [SerializeField]
    private GameObject upgradeArcherButton;
    [SerializeField]
    private Image placeArcherImage;

    private bool upgradedArcher = false;


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
}
