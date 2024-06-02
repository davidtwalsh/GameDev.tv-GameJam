using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    private List<Vector3> spawns;

    [SerializeField]
    private GameObject goblinPrefab;

    [SerializeField]
    private GameObject trollPrefab;

    [SerializeField]
    private GameObject armouredGoblinPrefab;

    [SerializeField]
    private GameObject demonPrefab;


    public static SpawnController Instance;

    private List<GameObject> monsters = new List<GameObject>();

    private bool hasStarted = false;

    private float gameTimer = 0f;

    private float checkWavesTimer = 0f;

    [SerializeField]
    private List<Wave> waves = new List<Wave>();

    [SerializeField]
    private float secondsLeft = 90f;

    [SerializeField]
    private TextMeshProUGUI timerTextMesh;
    [SerializeField]
    private GameObject timerPanel;

    [SerializeField]
    private TextMeshProUGUI waveTimerText;

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
            SpawnMonsterTest(goblinPrefab);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnMonsterTest(trollPrefab);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SpawnMonsterTest(armouredGoblinPrefab);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SpawnMonsterTest(demonPrefab);
        }

        if (hasStarted == true)
        {
            gameTimer += Time.deltaTime;
            checkWavesTimer += Time.deltaTime;

            if (waveTimerText != null)
            {
                waveTimerText.text = gameTimer.ToString();
            }

            if (checkWavesTimer > .3f)
            {
                Wave wave = GetWaveForTime((int)Mathf.Floor(gameTimer));
                if (wave != null)
                {
                    SpawnForWave(wave);
                    waves.Remove(wave);
                }
                checkWavesTimer = 0f;
            }


            secondsLeft -= Time.deltaTime;
            string timeLeftString = ConvertSecondsToMinutesAndSeconds(secondsLeft);
            timerTextMesh.text = timeLeftString;
            if (secondsLeft <= 0)
            {
                WinConditionController.Instance.WonGame();
                timerPanel.SetActive(false);
            }
        }
    }

    private void SpawnMonsterTest(GameObject prefab)
    {
        if (spawns == null || spawns.Count == 0)
        {
            return;
        }
        // Generate a random index within the bounds of the list
        int randomIndex = Random.Range(0, spawns.Count);
        GameObject monster = Instantiate(prefab, spawns[randomIndex], Quaternion.identity);
        monsters.Add(monster);
    }

    private void SpawnForWave(Wave wave)
    {
        List<Vector3> spawnsCopy = new List<Vector3>();
        foreach (Vector3 spawner in spawns)
        {
            spawnsCopy.Add(spawner);
        }

        foreach (MonsterTypeInWave monsterType in wave.monsters)
        {
            for (int i = 0; i < monsterType.monsterCount; i++)
            {
                if (spawnsCopy.Count == 0)
                {
                    Debug.LogError("Wave is too big! no more spawns");
                }
                int randomIndex = Random.Range(0, spawnsCopy.Count);
                GameObject monster = Instantiate(monsterType.monsterPrefab, spawns[randomIndex], Quaternion.identity);
                monsters.Add(monster);
                spawnsCopy.RemoveAt(randomIndex);
            }
        }
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

    public void StartGame()
    {
        hasStarted = true;
        timerPanel.SetActive(true);
    }

    public Wave GetWaveForTime(int time)
    {
        foreach (Wave wave in waves)
        {
            if (wave.waveTime == time)
            {
                return wave;
            }
        }
        return null;
    }

    private string ConvertSecondsToMinutesAndSeconds(float totalSeconds)
    {
        // Calculate minutes and seconds
        int minutes = Mathf.FloorToInt(totalSeconds / 60);
        int seconds = Mathf.FloorToInt(totalSeconds % 60);

        // Return the result as a string
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

[System.Serializable]
public class Wave
{
    public int waveTime;
    public List<MonsterTypeInWave> monsters;
    
}

[System.Serializable]
public class MonsterTypeInWave
{
    public GameObject monsterPrefab;
    public int monsterCount;
}
