using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UnitPlaceable : MonoBehaviour, Placeable
{
    [SerializeField]
    private GameObject unitPrefab;
    private Rigidbody2D rb;

    List<GameObject> colliders = new List<GameObject>();

    [SerializeField]
    private int cost;

    [SerializeField]
    private TextMeshProUGUI costText;

    private bool isOverTower = false;
    private Tower tower;

    [SerializeField]
    private bool isInvulnerable = false;

    AudioSource audioSource;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        costText.text = cost.ToString();
        gameObject.SetActive(false);
    }

    public bool IsPlaceable(Vector3 worldPosition)
    {
        bool result = false;
        if (colliders.Count == 0 || (colliders.Count == 1 && isOverTower == true))
        {
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    public void MoveWithMouse(Vector3 worldPosition)
    {
        // Update the position of the GameObject to follow the mouse cursor
        transform.position = worldPosition;
    }

    public void Place(Vector3 worldPosition)
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
        GameObject newObj = Instantiate(unitPrefab, transform.position, Quaternion.identity);
        if (isOverTower == false && isInvulnerable == false)
        {
            ObjectPlacer.Instance.GetPlayerAttackables().Add(newObj);
        }
        else if (isInvulnerable == true)
        {
            ObjectPlacer.Instance.GetFairies().Add(newObj);
            
        }

        ArcherAttacker archer = newObj.GetComponent<ArcherAttacker>();
        if (archer != null && UpgradeController.Instance.HasUpgradedArcher() == true)
        {
            UpgradeController.Instance.UpgradeArcher(archer);
        }
        WizardAttacker wizard = newObj.GetComponent<WizardAttacker>();
        if (wizard != null && UpgradeController.Instance.HasUpgradedWizard() == true)
        {
            UpgradeController.Instance.UpgradeWizard(wizard);
        }
        CrossbowAttacker crossbowMan = newObj.GetComponent<CrossbowAttacker>();
        if (crossbowMan != null && UpgradeController.Instance.HasUpgradedCrossbowMan() == true)
        {
            UpgradeController.Instance.UpgradeCrossbowMan(crossbowMan);
        }

        Fairy fairy = newObj.GetComponent<Fairy>();
        if (fairy != null && UpgradeController.Instance.HasUpgradedFairy() == true) 
        {
            UpgradeController.Instance.UpgradeFairy(fairy);
        }


        if (isOverTower == true)
        {
            bool placedUnitInTower = false;
            SpriteRenderer spriteRenderer = newObj.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = "Tower";
            }
            PlayerUnit playerUnit = newObj.GetComponent<PlayerUnit>();
            if (playerUnit != null)
            {
                playerUnit.SetIsInTower(true);
                if (tower != null)
                {
                    tower.AddUnit(playerUnit);
                    ObjectPlacer.Instance.GetToweredUnits().Add(newObj);
                    placedUnitInTower = true;
                }
            }
            if (placedUnitInTower == false)
            {
                ObjectPlacer.Instance.GetPlayerAttackables().Add(newObj);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (colliders.Contains(collision.gameObject) == false)
        {
            colliders.Add(collision.gameObject);
        }
        if (collision.tag == "Tower")
        {
            isOverTower = true;
            if (collision.gameObject.transform.parent != null)
            {
                Tower t = collision.transform.parent.GetComponent<Tower>();
                if (t != null)
                {
                    tower = t;
                }
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Clean up possible destroyed gameObjects
        List<GameObject> tempNoNulls = new List<GameObject>();
        foreach (GameObject obj in colliders)
        {
            if (obj != null)
            {
                tempNoNulls.Add(obj);
            }
        }
        if (colliders.Contains(collision.gameObject) == true)
        {
            colliders.Remove(collision.gameObject);
        }
        
        if (collision.tag == "Tower")
        {
            isOverTower = false;
            tower = null;
        }
    }
    public int GetCost()
    {
        return cost;
    }
}
