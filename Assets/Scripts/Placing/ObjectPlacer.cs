using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    private GameObject activePlacerGhost;
    private Placeable activePlaceable;
    private SpriteRenderer activePlaceGhostSpriteRenderer;

    [SerializeField]
    private GameObject wallPlacerGhost;
    [SerializeField]
    private GameObject archerPlacerGhost;

    private bool placingObject = false;
    private bool canPlaceObject = false;

    [SerializeField]
    private Color placableColor;
    [SerializeField]
    private Color nonPlacableColor;

    public static ObjectPlacer Instance;

    private List<GameObject> playerAttackables = new List<GameObject>();

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (activePlacerGhost != null)
            {
                activePlacerGhost.SetActive(false);
            }
            placingObject = true;
            activePlacerGhost = wallPlacerGhost;
            activePlacerGhost.SetActive(true);
            activePlaceable = activePlacerGhost.GetComponent<Placeable>();
            activePlaceGhostSpriteRenderer = activePlacerGhost.GetComponent<SpriteRenderer>();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            if (activePlacerGhost != null)
            {
                activePlacerGhost.SetActive(false);
            }
            placingObject = true;
            activePlacerGhost = archerPlacerGhost;
            activePlacerGhost.SetActive(true);
            activePlaceable = activePlacerGhost.GetComponent<Placeable>();
            activePlaceGhostSpriteRenderer = activePlacerGhost.GetComponent<SpriteRenderer>();    
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

            canPlaceObject = activePlaceable.IsPlaceable(worldPosition);
            if (canPlaceObject == true)
            {
                activePlaceGhostSpriteRenderer.color = placableColor;
            }
            else
            {
                activePlaceGhostSpriteRenderer.color = nonPlacableColor;
            }

            if (Input.GetMouseButtonDown(0) && canPlaceObject == true)
            {
                activePlaceable.Place(worldPosition);
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


}
