using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TowerPlaceable : MonoBehaviour, Placeable
{
    [SerializeField]
    private GameObject towerPrefab;
    private Rigidbody2D rb;

    List<GameObject> colliders = new List<GameObject>();

    [SerializeField]
    private int cost;

    [SerializeField]
    private TextMeshProUGUI costText;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        costText.text = cost.ToString();
        gameObject.SetActive(false);
    }

    public bool IsPlaceable(Vector3 worldPosition)
    {
        bool result = false;
        if (colliders.Count == 0)
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
        GameObject newObj = Instantiate(towerPrefab, transform.position, Quaternion.identity);
        ObjectPlacer.Instance.GetPlayerAttackables().Add(newObj);
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (colliders.Contains(collision.gameObject) == false)
        {
            colliders.Add(collision.gameObject);
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
    }
    public int GetCost()
    {
        return cost;
    }
}
