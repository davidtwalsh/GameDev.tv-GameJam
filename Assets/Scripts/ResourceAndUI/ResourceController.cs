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
            DontDestroyOnLoad(this.gameObject); // Optional: Don't destroy this object when loading new scenes
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
}
