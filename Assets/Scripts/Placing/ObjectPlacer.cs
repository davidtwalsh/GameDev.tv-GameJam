using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectPlacer : MonoBehaviour
{
    private GameObject activePlacerGhost;
    private Placeable activePlaceable;
    private SpriteRenderer activePlaceGhostSpriteRenderer;

    [SerializeField]
    private GameObject wallPlacerGhost;
    [SerializeField]
    private GameObject archerPlacerGhost;
    [SerializeField]
    private GameObject wizardPlacerGhost;
    [SerializeField]
    private GameObject crossbowManPlacerGhost;
    [SerializeField]
    private GameObject towerPlacerGhost;
    [SerializeField]
    private GameObject fairyPlacerGhost;

    private bool placingObject = false;
    private bool canPlaceObject = false;

    [SerializeField]
    private Color placableColor;
    [SerializeField]
    private Color nonPlacableColor;

    public static ObjectPlacer Instance;

    private List<GameObject> playerAttackables = new List<GameObject>();

    private List<GameObject> toweredUnits = new List<GameObject>();

    private bool canAfford = true;

    private List<GameObject> fairies = new List<GameObject>();


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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopPlacing();
        }

        if (placingObject == true)
        {
            // Get the position of the mouse cursor in screen coordinates
            Vector3 mousePosition = Input.mousePosition;

            // Convert the screen coordinates to world coordinates
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Keep the Z position constant (optional)
            worldPosition.z = 0f;

            activePlaceable.MoveWithMouse(worldPosition);

            if (ResourceController.Instance.CanSpendCoins(activePlaceable.GetCost()) == true)
            {
                canAfford = true;
            }
            else
            {
                canAfford = false;
            }

            canPlaceObject = activePlaceable.IsPlaceable(worldPosition);
            if (canPlaceObject == true && canAfford == true)
            {
                activePlaceGhostSpriteRenderer.color = placableColor;
            }
            else if (canPlaceObject == false || canAfford == false) 
            {
                activePlaceGhostSpriteRenderer.color = nonPlacableColor;
            }

            if (IsPointerOverUIObject() == true)
            {
                canPlaceObject = false;
                activePlacerGhost.SetActive(false);
            }
            else
            {
                activePlacerGhost.SetActive(true);
            }

            if (Input.GetMouseButtonDown(0) && canPlaceObject == true && canAfford == true)
            {
                activePlaceable.Place(worldPosition);
                ResourceController.Instance.SpendCoins(activePlaceable.GetCost());
                if (fairies.Count > 0)
                {
                    fairyPlacerGhost.GetComponent<UnitPlaceable>().SetCost(10);
                }
            }
        }
    }

    public static bool IsValidPositionToPlace(TileType tile)
    {
        if (tile == TileType.Grass)
        {
            return true;
        }
        return false;
    }

    public List<GameObject> GetPlayerAttackables()
    {
        return playerAttackables;
    }

    public List<GameObject> GetToweredUnits()
    {
        return toweredUnits;
    }

    private void SetUpNewGhost(GameObject newGhost)
    {
        placingObject = true;
        activePlacerGhost = newGhost;
        activePlacerGhost.SetActive(true);
        activePlaceable = activePlacerGhost.GetComponent<Placeable>();
        activePlaceGhostSpriteRenderer = activePlacerGhost.GetComponent<SpriteRenderer>();
    }

    private void CleanUpOldGhost()
    {
        if (activePlacerGhost != null)
        {
            activePlacerGhost.SetActive(false);
        }
    }

    public void SetPlacingWall()
    {
        CleanUpOldGhost();
        SetUpNewGhost(wallPlacerGhost);
    }

    public void SetPlacingArcher()
    {
        CleanUpOldGhost();
        SetUpNewGhost(archerPlacerGhost);
    }

    public void SetPlacingWizard()
    {
        CleanUpOldGhost();
        SetUpNewGhost(wizardPlacerGhost);
    }

    public void SetPlacingCrossbowMan()
    {
        CleanUpOldGhost();
        SetUpNewGhost(crossbowManPlacerGhost);
    }

    public void SetPlacingTower()
    {
        CleanUpOldGhost();
        SetUpNewGhost(towerPlacerGhost);
    }

    public void SetPlacingFairy()
    {
        CleanUpOldGhost();
        SetUpNewGhost(fairyPlacerGhost);
    }

    bool IsPointerOverUIObject()
    {
        // Check if EventSystem exists
        if (EventSystem.current == null)
            return false;

        // Create PointerEventData to pass to the event system
        PointerEventData eventData = new PointerEventData(EventSystem.current);

        // Set eventData to current mouse position
        eventData.position = Input.mousePosition;

        // Create a list to receive all results of the raycast
        var results = new System.Collections.Generic.List<RaycastResult>();

        // Raycast into the UI
        EventSystem.current.RaycastAll(eventData, results);

        // Check if the raycast hit any UI elements
        return results.Count > 0;
    }

    public void StopPlacing()
    {
        CleanUpOldGhost();
        placingObject = false;
    }

    public void ResumePlacing()
    {
        if (activePlacerGhost != null)
        {
            activePlacerGhost.SetActive(true);
            placingObject = true;
        }
    }

    public List<GameObject> GetFairies()
    {
        return fairies;
    }
}
