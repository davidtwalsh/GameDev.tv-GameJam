using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public static ResourceController Instance;

    [SerializeField]
    private int numCoins = 100;

    [SerializeField]
    TextMeshProUGUI coinAmountText;

    [SerializeField]
    private GameObject coinPrefab;

    [SerializeField]
    private List<Coin> groundCoins = new List<Coin>();

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

    private void Update()
    {
        coinAmountText.text = numCoins.ToString();
    }

    public int GetNumCoins()
    {
        return numCoins;
    }

    public void SpendCoins(int spent)
    {
        numCoins -= spent;
    }

    public bool CanSpendCoins(int amountToSpend)
    {
        if (numCoins - amountToSpend < 0)
        {
            return false;
        }
        return true;
    }

    public GameObject GetCoinPrefab()
    {
        return coinPrefab;
    }

    public List<Coin> GetGroundCoins()
    { 
        return groundCoins;
    }

    public void AddCoin()
    {
        numCoins++;
    }

}
