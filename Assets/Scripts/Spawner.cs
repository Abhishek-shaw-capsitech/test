// using UnityEngine;

// public class Spawner : MonoBehaviour
// {
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }

// --- Spawner.cs ---
// using System.Collections;
// using UnityEngine;

// public class Spawner : MonoBehaviour
// {
//     [SerializeField] private GameObject obstaclePrefab;
//     [SerializeField] private float spawnDistance = 100f;

//     // Difficulty scaling
//     [Header("Difficulty")]
//     [SerializeField] private float baseSpawnInterval = 3.0f;
//     [SerializeField] private float baseSpeed = 10.0f;
//     [SerializeField] private float difficultyTimeStep = 10f; // Every 10s
//     [SerializeField] private float speedIncrease = 1.0f;
//     [SerializeField] private float spawnIntervalDecrease = 0.1f;
//     [SerializeField] private float minSpawnInterval = 0.8f;

//     private float currentSpeed;
//     private float currentSpawnInterval;
//     private Coroutine spawnCoroutine;
//     private float difficultyTimer;

//     public void StartSpawning()
//     {
//         currentSpeed = baseSpeed;
//         currentSpawnInterval = baseSpawnInterval;
//         difficultyTimer = 0f;

//         if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
//         spawnCoroutine = StartCoroutine(SpawnWaveRoutine());
//     }

//     public void StopSpawning()
//     {
//         if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
//     }

//     private void Update()
//     {
//         if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameManager.GameState.Playing)
//             return;

//         // Time-based Difficulty Scaling
//         difficultyTimer += Time.deltaTime;
//         if (difficultyTimer >= difficultyTimeStep)
//         {
//             difficultyTimer = 0f;
//             currentSpeed += speedIncrease;
//             currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - spawnIntervalDecrease);
//         }
//     }

//     private IEnumerator SpawnWaveRoutine()
//     {
//         while (true)
//         {
//             yield return new WaitForSeconds(currentSpawnInterval);
//             SpawnObstacle();
//         }
//     }

//     void SpawnObstacle()
//     {
//         // 1. Choose a random safe lane (0=Bottom, 1=Right, 2=Top, 3=Left)
//         int safeLaneIndex = Random.Range(0, 4);

//         // 2. Instantiate the prefab
//         Vector3 spawnPos = new Vector3(0, 0, spawnDistance);
//         GameObject obstacle = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);

//         // 3. Configure the prefab to have a safe gate
//         Transform safeGate = obstacle.transform.Find("SafeGate");

//         // Deactivate the solid wall for the safe lane and position the gate
//         switch (safeLaneIndex)
//         {
//             case 0: // Bottom
//                 obstacle.transform.Find("Wall_Bottom").gameObject.SetActive(false);
//                 safeGate.position = new Vector3(0, -4.5f, spawnDistance);
//                 safeGate.localScale = new Vector3(10, 1, 1); // Size of the bottom wall
//                 break;
//             case 1: // Right
//                 obstacle.transform.Find("Wall_Right").gameObject.SetActive(false);
//                 safeGate.position = new Vector3(4.5f, 0, spawnDistance);
//                 safeGate.localScale = new Vector3(1, 10, 1);
//                 break;
//             case 2: // Top
//                 obstacle.transform.Find("Wall_Top").gameObject.SetActive(false);
//                 safeGate.position = new Vector3(0, 4.5f, spawnDistance);
//                 safeGate.localScale = new Vector3(10, 1, 1);
//                 break;
//             case 3: // Left
//                 obstacle.transform.Find("Wall_Left").gameObject.SetActive(false);
//                 safeGate.position = new Vector3(-4.5f, 0, spawnDistance);
//                 safeGate.localScale = new Vector3(1, 10, 1);
//                 break;
//         }

//         // 4. Activate the gate and set the speed
//         safeGate.gameObject.SetActive(true);
//         obstacle.GetComponent<ObstacleMover>().speed = currentSpeed;
//     }
// }

// --- Spawner.cs ---
using System.Collections;
using System.Collections.Generic; // Needed for Lists
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController player; // Assign the PlayerRig
    [SerializeField] private GameObject[] obstaclePrefabs; // Drag all 3 prefabs here

    [Header("Spawning")]
    [SerializeField] private float spawnDistance = 100f;

    // Difficulty scaling
    [Header("Difficulty")]
    [SerializeField] private float baseSpawnInterval = 4.0f;
    [SerializeField] private float baseSpeed = 5.0f;
    [SerializeField] private float difficultyTimeStep = 10f;
    [SerializeField] private float speedIncrease = 1.0f;
    [SerializeField] private float spawnIntervalDecrease = 0.1f;
    [SerializeField] private float minSpawnInterval = 0.8f;

    // private float currentSpeed;
    // Public and static means any script can read this value
    public static float currentSpeed;
    private float currentSpawnInterval;

    private Coroutine spawnCoroutine;
    private float difficultyTimer;

    // Lane positions at spawn distance
    private readonly Vector3[] lanePositions = {
        new Vector3(0, -4.5f, 0),    // 0: Bottom
        new Vector3(4.5f, 0, 0),     // 1: Right
        new Vector3(0, 4.5f, 0),     // 2: Top
        new Vector3(-4.5f, 0, 0)     // 3: Left
    };

    public void StartSpawning()
    {
        currentSpeed = baseSpeed;
        currentSpawnInterval = baseSpawnInterval;
        difficultyTimer = 0f;

        if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
        spawnCoroutine = StartCoroutine(SpawnWaveRoutine());
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
    }

    private void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            return;

        // Difficulty Scaling
        difficultyTimer += Time.deltaTime;
        if (difficultyTimer >= difficultyTimeStep)
        {
            difficultyTimer = 0f;
            currentSpeed += speedIncrease;
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - spawnIntervalDecrease);
        }
    }

    private IEnumerator SpawnWaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSpawnInterval);
            SpawnWave();
        }
    }

    void SpawnWave()
    {
        // 1. Get the player's current shape. This is the "correct" shape.
        ShapeType correctShape = player.currentShape;

        // 2. Pick a random "safe lane" (0-3)
        int safeLaneIndex = Random.Range(0, 4);

        // 3. Spawn all four obstacles
        for (int i = 0; i < 4; i++) // 0=Bottom, 1=Right, 2=Top, 3=Left
        {
            GameObject prefabToSpawn;

            if (i == safeLaneIndex)
            {
                // This is the safe lane. Spawn the matching shape.
                prefabToSpawn = GetPrefabForShape(correctShape);
            }
            else
            {
                // This is a wrong lane. Spawn a non-matching shape.
                prefabToSpawn = GetRandomWrongPrefab(correctShape);
            }

            // 4. Instantiate at the correct lane position
            Vector3 spawnPos = lanePositions[i] + new Vector3(0, 0, spawnDistance);
            GameObject newObstacle = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

            // 5. Set its speed
            // newObstacle.GetComponent<Obstacle>().speed = currentSpeed;
        }
    }

    // --- Helper Functions ---
    GameObject GetPrefabForShape(ShapeType shape)
    {
        foreach (var prefab in obstaclePrefabs)
        {
            if (prefab.GetComponent<Obstacle>().shapeType == shape)
            {
                return prefab;
            }
        }
        return obstaclePrefabs[0]; // Fallback
    }

    GameObject GetRandomWrongPrefab(ShapeType correctShape)
    {
        List<GameObject> wrongPrefabs = new List<GameObject>();
        foreach (var prefab in obstaclePrefabs)
        {
            if (prefab.GetComponent<Obstacle>().shapeType != correctShape)
            {
                wrongPrefabs.Add(prefab);
            }
        }

        // Return a random one from the "wrong" list
        return wrongPrefabs[Random.Range(0, wrongPrefabs.Count)];
    }
}