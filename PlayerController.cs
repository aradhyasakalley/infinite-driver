using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private float maxSpeed;
    private float acceleration;
    private float deceleration;
    private float turnSpeed = 45.0f;
    private float currentSpeed = 0.0f;
    private float horizontalInput;
    private float forwardInput;
    public GameObject explosionEffect;
    public float explosionDuration = 1.0f;
    public float collisionForce = 1000.0f;
    public float upwardsForce = 500.0f;

    // Reference to the ChangeMesh script attached to the vehicle
    private ChangeMesh vehicleController;

    // Threshold for falling off the road
    public float fallThreshold = -10.0f;

    void Start()
    {
        // Get the ChangeMesh component attached to the vehicle
        vehicleController = GetComponent<ChangeMesh>();
        if (vehicleController == null)
        {
            Debug.LogError("ChangeMesh script not found on the player's vehicle GameObject!");
        }

        // Set vehicle parameters based on selected difficulty
        SetDifficultyParameters();
    }

    void SetDifficultyParameters()
    {
        string difficulty = PlayerPrefs.GetString("SelectedDifficulty", "Medium");

        Debug.Log("Selected Difficulty: " + difficulty); 

        switch (difficulty)
        {
            case "Easy":
                maxSpeed = 20.0f;
                turnSpeed = 45.0f;
                acceleration = 4.0f;
                deceleration = 10.0f;
                break;
            case "Medium":
                maxSpeed = 40.0f;
                turnSpeed = 35.0f;
                acceleration = 4.0f; 
                deceleration = 10.0f; 
                break;
            case "Hard":
                maxSpeed = 60.0f;
                turnSpeed = 25.0f;
                acceleration = 6.0f;
                deceleration = 8.0f; 
                break;
            default:
                Debug.LogError("Unknown difficulty setting.");
                break;
        }

        // Debug statements to check the applied parameters
        
    }

    void Update()
    {
        // Check if the vehicle has fallen off the road
        if (transform.position.y < fallThreshold)
        {
            Debug.Log("fallen off");
            SceneManager.LoadSceneAsync(4);
        }

        // Get input from the player
        forwardInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        // Accelerate or decelerate based on player input
        if (forwardInput > 0)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else if (forwardInput < 0)
        {
            currentSpeed -= deceleration * Time.deltaTime;
        }
        else
        {
            // Gradually stop the vehicle
            if (currentSpeed > 0)
            {
                currentSpeed -= deceleration * Time.deltaTime;
                if (currentSpeed < 0)
                {
                    currentSpeed = 0;
                }
            }
            else if (currentSpeed < 0)
            {
                currentSpeed += deceleration * Time.deltaTime;
                if (currentSpeed > 0)
                {
                    currentSpeed = 0;
                }
            }
        }

        // Clamp the current speed to the max speed
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        // Rotate the vehicle if it is moving
        if (currentSpeed != 0)
        {
            transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);
        }

        // Move the vehicle forward based on its current speed
        transform.Translate(Vector3.forward * Time.deltaTime * currentSpeed);

        Debug.Log("Max Speed: " + maxSpeed);
        Debug.Log("Current Speed: " + currentSpeed);
        Debug.Log("Turning Speed: " + turnSpeed);
        Debug.Log("Acceleration: " + acceleration);
        Debug.Log("Deceleration: " + deceleration);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with an object tagged as "Obstacle"
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Instantiate explosion effect
            Instantiate(explosionEffect, collision.contacts[0].point, Quaternion.identity);

            // Stop the vehicle
            currentSpeed = 0;

            // Wait for a duration and load the game over scene
            StartCoroutine(WaitAndLoadScene(explosionDuration));
        }

        // Apply force to the obstacle on collision
        Rigidbody obstacleRigidbody = collision.gameObject.GetComponent<Rigidbody>();
        if (obstacleRigidbody != null)
        {
            // Calculate the force direction and apply force to the obstacle
            Vector3 forceDirection = Quaternion.AngleAxis(75, transform.right) * transform.forward;
            forceDirection = forceDirection.normalized;
            obstacleRigidbody.AddForce(forceDirection * collisionForce, ForceMode.Impulse);
        }
    }

    private IEnumerator WaitAndLoadScene(float waitTime)
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(waitTime);

        // Load the game over scene
        SceneManager.LoadSceneAsync(4);
    }

    // Method to change the vehicle type
    void ChangeVehicleType(ChangeMesh.VehicleType newVehicleType)
    {
        if (vehicleController != null)
        {
            vehicleController.ChangeVehicleType(newVehicleType);
        }
        else
        {
            Debug.LogError("ChangeMesh script not found on the player's vehicle GameObject!");
        }
    }
}
