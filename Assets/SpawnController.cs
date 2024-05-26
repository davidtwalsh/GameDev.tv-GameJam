using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField]
    private List<Transform> spawns;

    [SerializeField]
    private GameObject goblinPrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SpawnGoblins();
        }
    }

    private void SpawnGoblins()
    {
        foreach (Transform t in spawns)
        {
            GameObject monster = Instantiate(goblinPrefab, t.position, Quaternion.identity);
        }
    }
}
