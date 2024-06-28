using UnityEngine;
using System.Collections.Generic;

public class RoadManager : MonoBehaviour
{
    public GameObject roadPrefab;
    public GameObject obstaclePrefab;
    public GameObject barrelPrefab;
    public GameObject coinPrefab;
    public GameObject starPrefab;
    public GameObject magnetPrefab;
    public int numberOfSegments = 5;
    public float segmentLength = 20.0f;
    public Transform player;

    private List<GameObject> roadSegments = new List<GameObject>();
    private float spawnZ = 0.0f;
    private float safeZone = 30.0f;

    private float obstacleMinDistance;
    private float lastObstacleZ = -1.0f;

    private float[] obstacleXPositions = { -10.0f, 0.0f, 10.0f };
    private float[] barrelXPositions = { -6.0f, 0.0f, 6.0f };
    private int cycleCount = 0;
    private float laneCycleGap = 40.0f; 
    private ChangeMesh.VehicleType currentVehicleType;

    void Start()
    {
        // Initialize parameters and spawn initial road segments
        SetDifficultyParameters();

        // Get the current vehicle type from the player's vehicle
        ChangeMesh vehicleController = player.GetComponentInChildren<ChangeMesh>();
        if (vehicleController != null)
        {
            currentVehicleType = vehicleController.vehicleType;
        }
        else
        {
            Debug.LogError("ChangeMesh script not found on the player's vehicle GameObject!");
        }

        for (int i = 0; i < numberOfSegments; i++)
        {
            SpawnRoadSegment();
        }
    }

    void Update()
    {
        // Check if more road segments need to be spawned
        if (player.position.z - safeZone > (spawnZ - numberOfSegments * segmentLength))
        {
            SpawnRoadSegment();
            DeleteOldSegment();
        }

        // Check for obstacle spawning
        SpawnObstacles();
    }

    void SetDifficultyParameters()
    {
        string difficulty = PlayerPrefs.GetString("SelectedDifficulty", "Medium");

        switch (difficulty)
        {
            case "Easy":
                obstacleMinDistance = 50.0f;
                break;
            case "Medium":
                obstacleMinDistance = 30.0f;
                break;
            case "Hard":
                obstacleMinDistance = 10.0f;
                break;
            default:
                Debug.LogError("Unknown difficulty setting.");
                obstacleMinDistance = 20.0f; // Default distance
                break;
        }
    }

    void SpawnRoadSegment()
    {
        // Instantiate a road segment and add it to the list
        GameObject roadSegment = Instantiate(roadPrefab, Vector3.forward * spawnZ, Quaternion.Euler(0, 90, 0));
        roadSegments.Add(roadSegment);
        spawnZ += segmentLength;
    }

    void DeleteOldSegment()
    {
        // Delete the oldest road segment when it's no longer visible
        Destroy(roadSegments[0]);
        roadSegments.RemoveAt(0);
    }

    void SpawnObstacles()
    {
        float obstacleZ = spawnZ - segmentLength;

        // Check if the minimum distance between obstacles has been met
        if (obstacleZ - lastObstacleZ < obstacleMinDistance)
        {
            return;
        }

        // Randomly choose two out of three lanes
        int[] chosenLanes = new int[2];
        chosenLanes[0] = Random.Range(0, 3);
        do
        {
            chosenLanes[1] = Random.Range(0, 3);
        } while (chosenLanes[1] == chosenLanes[0]);

        // Spawn a line of coins and an obstacle on the chosen lanes
        for (int i = 0; i < 2; i++)
        {
            int laneIndex = chosenLanes[i];
            float spawnX;

            if (i == 0)
            {
                // Spawn a line of coins
                spawnX = obstacleXPositions[laneIndex];
                SpawnCoinLine(spawnX, obstacleZ);
            }
            else
            {
                // Spawn an obstacle
                spawnX = obstacleXPositions[laneIndex];
                GameObject spawnPrefab = obstaclePrefab;
                Instantiate(spawnPrefab, new Vector3(spawnX, 0.5f, obstacleZ), Quaternion.Euler(0, 180, 0));
            }
        }

        // Update lastObstacleZ to account for the gap between cycles
        lastObstacleZ = obstacleZ + laneCycleGap;

        // Track cycles for stars and magnets
        cycleCount++;
        Debug.Log("Cycle count : " + cycleCount);
        // After every 2 cycles, spawn a star
        if (cycleCount % 2 == 0)
        {
            int starLaneIndex = Random.Range(0, 3);
            float spawnX = obstacleXPositions[starLaneIndex];
            GameObject spawnPrefab = starPrefab;
            Instantiate(spawnPrefab, new Vector3(spawnX, 2.0f, obstacleZ), Quaternion.Euler(0, 90, 90));
        }

        // After 6 cycles, spawn a magnet
        if (cycleCount == 6)
        {
            int magnetLaneIndex = Random.Range(0, 3);
            float spawnX = obstacleXPositions[magnetLaneIndex];
            GameObject spawnPrefab = magnetPrefab;
            Instantiate(spawnPrefab, new Vector3(spawnX, 1.5f, obstacleZ), Quaternion.Euler(0, 90, 0));

            // Reset cycle count after spawning magnet
            cycleCount = 0;
        }
    }

    void SpawnCoinLine(float startX, float startZ)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 spawnPosition = new Vector3(startX, 1.0f, startZ + i * 3.0f);
            Instantiate(coinPrefab, spawnPosition, Quaternion.Euler(0, 0, 90));
        }
    }
}
