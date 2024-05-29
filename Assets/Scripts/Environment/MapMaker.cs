using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    [SerializeField]
    private int ySize;
    [SerializeField]
    private int xSize;

    private TileType[,] map;

    [SerializeField]
    private GameObject grassPrefab;
    [SerializeField]
    private GameObject stonePrefab;

    public static MapMaker Instance;

    public Dictionary<(int, int), Wall> walls = new Dictionary<(int, int), Wall>();

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

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
        GenerateTiles();
    }

    private void GenerateMap()
    {
        map = new TileType[xSize,ySize];
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (x == 0 || x == 1 || y == 0 || y == 1 || x == xSize-1 || x == xSize -2 || y == ySize - 1 || y == ySize - 2)
                {
                    map[x, y] = TileType.Boundary;
                }
                else
                {
                    map[x, y] = TileType.Grass;
                }
            }
        }
    }

    private void GenerateTiles()
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject prefab = null;
                if (map[x,y] == TileType.Boundary)
                {
                    prefab = stonePrefab;
                }
                else if (map[x,y] == TileType.Grass)
                {
                    prefab = grassPrefab;
                }
                GameObject obj = Instantiate(prefab, new Vector2(x, y), Quaternion.identity);
                obj.transform.parent = transform;
            }
        }
    }

    public void SetTileTypeForCell(int x, int y, TileType tileType)
    {
        if (x < 0 || y < 0 || x >= xSize || y >= ySize)
        {
            //Debug.LogError($"invalid input in SetTileTypeForCell for MapMaker, x: {x}, y: {y}");
            return;
        }
        map[x, y] = tileType;
    }

    public TileType GetTileTypeForCell(int x, int y)
    {
        if (x < 0 || y < 0 || x >= xSize || y >= ySize)
        {
            //Debug.LogError($"invalid input in GetTileTypeForCell for MapMaker, x: {x}, y: {y}");
            return TileType.Unknown;
        }
        return map[x, y]; 
    }

    public int getXSize()
    {
        return xSize;
    }
    public int getYSize()
    {
        return ySize;
    }
}


public enum TileType
{
    Boundary,
    Grass,
    Wall,
    Unknown
}