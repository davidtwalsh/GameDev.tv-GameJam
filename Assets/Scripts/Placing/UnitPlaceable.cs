using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitPlaceable : MonoBehaviour, Placeable
{
    [SerializeField]
    private GameObject unitPrefab;
    private Rigidbody2D rb;

    List<GameObject> colliders = new List<GameObject>();

    [SerializeField]
    private int cost;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        GameObject newObj = Instantiate(unitPrefab, transform.position, Quaternion.identity);
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
