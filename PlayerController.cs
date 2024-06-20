using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private float maxSpeed = 40.0f;
    private float acceleration = 10.0f;
    private float deceleration = 10.0f;
    private float turnSpeed = 45.0f;
    private float currentSpeed = 0.0f;
    private float horizontalInput;
    private float forwardInput;
    public GameObject explosionEffect;
    public float explosionDuration = 1.0f;
    public float difficultyIncreaseRate = 0.1f;
    public float maxSpeedIncreaseRate = 0.5f;
    public float accelerationIncreaseRate = 0.2f;
    public float collisionForce = 1000.0f;
    public float upwardsForce = 500.0f;

    // Reference to the VehicleController script attached to the vehicle
    private ChangeMesh vehicleController;

    void Start()
    {
        // Get the VehicleController component attached to the vehicle
        vehicleController = GetComponent<ChangeMesh>();
        if (vehicleController == null)
        {
            Debug.LogError("VehicleController script not found on the player's vehicle GameObject!");
        }
    }

    void Update()
    {
        // Get input from the player
        forwardInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        // Increase max speed and acceleration over time
        maxSpeed += maxSpeedIncreaseRate * Time.deltaTime * difficultyIncreaseRate;
        acceleration += accelerationIncreaseRate * Time.deltaTime * difficultyIncreaseRate;

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
        SceneManager.LoadSceneAsync(3);
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
            Debug.LogError("VehicleController script not found on the player's vehicle GameObject!");
        }
    }
}
