using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditionController : MonoBehaviour
{
    public static WinConditionController Instance;

    [SerializeField]
    private GameObject lostPanel;

    [SerializeField]
    private GameObject wonPanel;

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

        yield return new WaitForSeconds(12f);

        SceneController.Instance.LoadScene("LostScene");
    }

    public void WonGame()
    {
        StartCoroutine (WonGameCoroutine());
    }

    IEnumerator WonGameCoroutine()
    {
        wonPanel.SetActive(true);

        List<GameObject> monsters = SpawnController.Instance.GetMonsters();
        foreach (GameObject monster in monsters)
        {
            Enemy enemy = monster.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.SetState(EnemyState.Fleeing);
            }
        }

        yield return new WaitForSeconds(12f);

        SceneController.Instance.LoadScene("WonScene");

    }
}
