using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructurePlaceable : MonoBehaviour, Placeable
{
    [SerializeField]
    private GameObject structurePrefab;

    List<GameObject> colliders = new List<GameObject>();

    private Rigidbody2D rb;

    [SerializeField]
    private int cost;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public bool IsPlaceable(Vector3 worldPosition)
    {
        bool result = false;

        worldPosition.x = Mathf.FloorToInt(worldPosition.x + .5f);
        worldPosition.y = Mathf.FloorToInt(worldPosition.y + .5f);

        int xMapPosition = (int)worldPosition.x;
        int yMapPosition = (int)worldPosition.y;

        TileType tileTypeAtPos = MapMaker.Instance.GetTileTypeForCell(xMapPosition, yMapPosition);
        if (ObjectPlacer.IsValidPositionToPlace(tileTypeAtPos) == true)
        {
            result = true;
        }
        else
        {
            result = false;
        }
        if (colliders.Count > 0)
        {
            result = false;
        }
        return result;
    }

    public void Place(Vector3 worldPosition)
    {
        worldPosition.x = Mathf.FloorToInt(worldPosition.x + .5f);
        worldPosition.y = Mathf.FloorToInt(worldPosition.y + .5f);

        int xMapPosition = (int)worldPosition.x;
        int yMapPosition = (int)worldPosition.y;
        GameObject newObj = Instantiate(structurePrefab, transform.position, Quaternion.identity);
        
        ObjectPlacer.Instance.GetPlayerAttackables().Add(newObj);

        MapMaker.Instance.SetTileTypeForCell(xMapPosition, yMapPosition, TileType.Wall);
        Wall wall = newObj.GetComponent<Wall>();
        if (wall != null)
        {
            wall.SetPosition(xMapPosition, yMapPosition);
            if (UpgradeController.Instance.HasUpgradedWalls())
            {
                UpgradeController.Instance.UpgradeWall(wall);
            }
            wall.SetSprite();
            wall.UpdateOtherSprites();
            MapMaker.Instance.walls.Add((xMapPosition, yMapPosition), wall);
        }
    }

    public void MoveWithMouse(Vector3 worldPosition)
    {
        worldPosition.x = Mathf.FloorToInt(worldPosition.x + .5f);
        worldPosition.y = Mathf.FloorToInt(worldPosition.y + .5f);

        // Update the position of the GameObject to follow the mouse cursor
        transform.position = worldPosition;
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
