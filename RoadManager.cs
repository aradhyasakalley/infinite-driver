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
                obstacleMinDistance = 10.0f; 
                break;
            case "Medium":
                obstacleMinDistance = 7.0f; 
                break;
            case "Hard":
                obstacleMinDistance = 5.0f; 
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
        float spawnChance = Random.value;

        if (spawnChance < 0.4f)
        {
            // Spawn a coin with rotation Quaternion.Euler(0, 90, 0) and position up on the y-axis
            spawnPrefab = coinPrefab;
            spawnX = obstacleXPositions[Random.Range(0, obstacleXPositions.Length)];
            Instantiate(spawnPrefab, new Vector3(spawnX, 1.0f, obstacleZ), Quaternion.Euler(0, 0, 90));
        }
        else if (spawnChance < 0.7f)
        {
            // Spawn an obstacle with rotation Quaternion.Euler(0, 180, 0) and default y-axis position
            spawnPrefab = obstaclePrefab;
            spawnX = obstacleXPositions[Random.Range(0, obstacleXPositions.Length)];
            Instantiate(spawnPrefab, new Vector3(spawnX, 0.5f, obstacleZ), Quaternion.Euler(0, 180, 0));
        }
        else
        {
            // Spawn a barrel with rotation Quaternion.Euler(0, 180, 0) and default y-axis position
            spawnPrefab = barrelPrefab;
            spawnX = barrelXPositions[Random.Range(0, barrelXPositions.Length)];
            Instantiate(spawnPrefab, new Vector3(spawnX, 0.5f, obstacleZ), Quaternion.Euler(0, 180, 0));
        }

        lastObstacleZ = obstacleZ;
    }



}
