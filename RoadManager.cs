using UnityEngine;
using System.Collections.Generic;

public class RoadManager : MonoBehaviour
{
    public GameObject roadPrefab;
    public GameObject obstaclePrefab;
    public GameObject barrelPrefab; 
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

    void Start()
    {
        SetDifficultyParameters();

        for (int i = 0; i < numberOfSegments; i++)
        {
            SpawnRoadSegment();
        }
    }

    void Update()
    {
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
                obstacleMinDistance = 30.0f; 
                break;
            case "Medium":
                obstacleMinDistance = 20.0f; 
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
        GameObject roadSegment = Instantiate(roadPrefab, Vector3.forward * spawnZ, Quaternion.Euler(0, 90, 0));
        roadSegments.Add(roadSegment);
        spawnZ += segmentLength;
    }

    void DeleteOldSegment()
    {
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
        if (Random.value > 0.5f)
        {
            // Spawn an obstacle
            spawnPrefab = obstaclePrefab;
            spawnX = obstacleXPositions[Random.Range(0, obstacleXPositions.Length)];
        }
        else
        {
            // Spawn a barrel
            spawnPrefab = barrelPrefab;
            spawnX = barrelXPositions[Random.Range(0, barrelXPositions.Length)];
        }

        GameObject spawnedObject = Instantiate(spawnPrefab, new Vector3(spawnX, 0.5f, obstacleZ), Quaternion.Euler(0, 180, 0));
        lastObstacleZ = obstacleZ;
    }
}
