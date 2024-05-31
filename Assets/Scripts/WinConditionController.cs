using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditionController : MonoBehaviour
{
    public static WinConditionController Instance;

    [SerializeField]
    private GameObject lostPanel;

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

    public void LostGame()
    {
        StartCoroutine(LostGameCoroutine());
    }

    IEnumerator LostGameCoroutine()
    {
        lostPanel.SetActive(true);

        yield return new WaitForSeconds(8f);

        SceneController.Instance.LoadScene("LostScene");
    }

    public void WonGame()
    {
        Debug.Log("Game won");
    }
}
