using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    private List<Vector3> spawns;

    [SerializeField]
    private GameObject goblinPrefab;

    public static SpawnController Instance;

    private List<GameObject> monsters = new List<GameObject>();

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

    private void Start()
    {
        GetSpawns();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SpawnGoblin();
        }
    }

    private void SpawnGoblin()
    {
        if (spawns == null || spawns.Count == 0)
        {
            return;
        }
        // Generate a random index within the bounds of the list
        int randomIndex = Random.Range(0, spawns.Count);
        GameObject monster = Instantiate(goblinPrefab, spawns[randomIndex], Quaternion.identity);
        monsters.Add(monster);
        
    }
    private void GetSpawns()
    {
        spawns = new List<Vector3>();
        int x = MapMaker.Instance.getXSize();
        int y = MapMaker.Instance.getYSize();
        for (int i = 0; i < x; i++)
        {
            spawns.Add(new Vector3(i, 0, 0));
            spawns.Add(new Vector3(i, y-1, 0));
        }
        for (int i = 0; i < y; i++)
        {
            spawns.Add(new Vector3(0, i, 0));
            spawns.Add(new Vector3(x-1, i, 0));
        }
    }

    public List<GameObject> GetMonsters()
    {
        return monsters;
    }
}
