using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{

    [SerializeField]
    private GameObject wallPlacerGhost;
    [SerializeField]
    private GameObject wallPrefab;
    private bool placingObject = false;
    private bool canPlaceObject = false;

    [SerializeField]
    private Color placableColor;
    [SerializeField]
    private Color nonPlacableColor;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            placingObject = true;
            wallPlacerGhost.SetActive(true);
            Debug.Log("here");
        }
        if (placingObject == true)
        {
            // Get the position of the mouse cursor in screen coordinates
            Vector3 mousePosition = Input.mousePosition;

            // Convert the screen coordinates to world coordinates
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Keep the Z position constant (optional)
            worldPosition.z = 0f;

            worldPosition.x = Mathf.FloorToInt(worldPosition.x + .5f);
            worldPosition.y = Mathf.FloorToInt(worldPosition.y + .5f);

            // Update the position of the GameObject to follow the mouse cursor
            wallPlacerGhost.transform.position = worldPosition;

            int xMapPosition = (int)worldPosition.x;
            int yMapPosition = (int)worldPosition.y;

            TileType tileTypeAtPos = MapMaker.Instance.GetTileTypeForCell(xMapPosition, yMapPosition);
            if (IsValidPositionToPlace(tileTypeAtPos) == true)
            {
                canPlaceObject = true;
                wallPlacerGhost.GetComponent<SpriteRenderer>().color = placableColor;
            }
            else
            {
                canPlaceObject = false;
                wallPlacerGhost.GetComponent<SpriteRenderer>().color = nonPlacableColor;
            }

            if (Input.GetMouseButtonDown(0) && canPlaceObject == true)
            {
                GameObject newObj = Instantiate(wallPrefab, wallPlacerGhost.transform.position, Quaternion.identity);
                newObj.transform.parent = transform;
                MapMaker.Instance.SetTileTypeForCell(xMapPosition, yMapPosition, TileType.Wall);
                Wall wall = newObj.GetComponent<Wall>();
                if (wall != null)
                {
                    wall.SetPosition(xMapPosition, yMapPosition);
                    wall.SetSprite();
                    wall.UpdateOtherSprites();
                    MapMaker.Instance.walls.Add((xMapPosition, yMapPosition),wall);
                }
            }
        }
    }

    private bool IsValidPositionToPlace(TileType tile)
    {
        if (tile == TileType.Grass)
        {
            return true;
        }
        return false;
    }


}
