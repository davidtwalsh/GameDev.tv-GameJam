using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairController : MonoBehaviour
{
    public static RepairController Instance;

    [SerializeField]
    private GameObject wrenchObj;
    private bool isRepairing = false;

    [SerializeField]
    private Color normalWallColor;
    [SerializeField]
    private Color selectedWallColor;

    private Wall selectedWall;
    private int selectedX = -999999;
    private int selectedY = -999999;

    [SerializeField]
    private int repairCost = 2;

    [SerializeField]
    Vector3 wrenchOffset;

    [SerializeField]
    private GameObject goldCostObj;


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
        if (isRepairing == true)
        {
            Vector3 worldPosition = GetMousePosition();
            Vector3 wrenchPosition = worldPosition + wrenchOffset;
            // Set the position of the sprite to the converted mouse position
            wrenchObj.transform.position = wrenchPosition;

            worldPosition.x = Mathf.FloorToInt(worldPosition.x + .5f);
            worldPosition.y = Mathf.FloorToInt(worldPosition.y + .5f);

            int xMapPosition = (int)worldPosition.x;
            int yMapPosition = (int)worldPosition.y;


            if (selectedWall != null && (xMapPosition != selectedX || yMapPosition != selectedY))
            {
                selectedWall.GetSpriteRenderer().color = normalWallColor;
                selectedWall = null;
                goldCostObj.SetActive(false);
            }
            else if (selectedWall != null)
            {
                selectedWall.GetSpriteRenderer().color = selectedWallColor;
                goldCostObj.SetActive(true);
            }
            if (MapMaker.Instance.walls.ContainsKey((xMapPosition, yMapPosition)) && selectedWall == null)
            {
                Wall wall = MapMaker.Instance.walls[(xMapPosition, yMapPosition)];
                if (wall.GetEntityStatus().IsFullHP() == false)
                {
                    wall.GetSpriteRenderer().color = selectedWallColor;
                    selectedWall = wall;
                    selectedX = xMapPosition;
                    selectedY = yMapPosition;
                    goldCostObj.SetActive(true);
                }
            }

            if (selectedWall != null && Input.GetMouseButtonDown(0) && ResourceController.Instance.CanSpendCoins(repairCost))
            {
                RepairWall();
            }
        }
    }

    public void StartRepairing()
    {
        isRepairing = true;
        wrenchObj.SetActive(true);
    }

    public void EndRepairing()
    {
        isRepairing = false;
        wrenchObj.SetActive(false);
    }

    private void RepairWall()
    {
        ResourceController.Instance.SpendCoins(repairCost);
        EntityStatus status = selectedWall.GetEntityStatus();
        status.RestoreHP();
        selectedWall.CheckIfNeedToUpdateWallSprite();
        selectedWall.GetSpriteRenderer().color = normalWallColor;
        selectedWall = null;
        goldCostObj.SetActive(false);
    }

    private Vector3 GetMousePosition()
    {
        // Get the current mouse position in screen coordinates
        Vector3 mousePosScreen = Input.mousePosition;

        // Convert the mouse position from screen coordinates to world coordinates
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePosScreen);
        mousePosWorld.z = 0f; // Ensure the sprite stays on the same z-coordinate

        return mousePosWorld;
    }

}
