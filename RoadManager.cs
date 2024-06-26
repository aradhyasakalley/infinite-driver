using UnityEngine;
using System.Collections.Generic;

public class RoadManager : MonoBehaviour
{
    public GameObject roadPrefab;
    public GameObject obstaclePrefab;
    public GameObject barrelPrefab;
    public GameObject coinPrefab;
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

        if (obstacleZ - lastObstacleZ < obstacleMinDistance)
        {
            return;
        }

        GameObject spawnPrefab;
        float spawnX;
        float spawnChance = Random.value;

        // Adjust obstacle distance based on vehicle type
        float obstacleDistanceModifier = 1.0f;
        switch (currentVehicleType)
        {
            case ChangeMesh.VehicleType.Bus:
                obstacleDistanceModifier = 3.0f;
                break;
            case ChangeMesh.VehicleType.Tank:
                obstacleDistanceModifier = 2.0f;
                break;
            case ChangeMesh.VehicleType.Van:
                obstacleDistanceModifier = 1.5f;
                break;
        }

        float adjustedObstacleMinDistance = obstacleMinDistance * obstacleDistanceModifier;

        if (obstacleZ - lastObstacleZ >= adjustedObstacleMinDistance)
        {
            if (spawnChance < 0.6f)
            {
                // Spawn a coin (increased chance compared to obstacles and barrels)
                spawnPrefab = coinPrefab;
                spawnX = obstacleXPositions[Random.Range(0, obstacleXPositions.Length)];
                Instantiate(spawnPrefab, new Vector3(spawnX, 1.0f, obstacleZ), Quaternion.Euler(0, 0, 90));
            }
            else if (spawnChance < 0.8f)
            {
                // Spawn an obstacle
                spawnPrefab = obstaclePrefab;
                spawnX = obstacleXPositions[Random.Range(0, obstacleXPositions.Length)];
                Instantiate(spawnPrefab, new Vector3(spawnX, 0.5f, obstacleZ), Quaternion.Euler(0, 180, 0));
            }
            else
            {
                // Spawn a barrel
                spawnPrefab = barrelPrefab;
                spawnX = barrelXPositions[Random.Range(0, barrelXPositions.Length)];
                Instantiate(spawnPrefab, new Vector3(spawnX, 0.5f, obstacleZ), Quaternion.Euler(0, 180, 0));
            }

            lastObstacleZ = obstacleZ;
        }
    }
}
