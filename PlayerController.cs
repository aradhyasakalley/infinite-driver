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

    public float collisionForce =   1000.0f;
    public float upwardsForce = 500.0f;
    // Update is called once per frame
    void Update()
    {
        forwardInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");


        maxSpeed += maxSpeedIncreaseRate * Time.deltaTime * difficultyIncreaseRate;
        acceleration += accelerationIncreaseRate * Time.deltaTime * difficultyIncreaseRate;


        // add or remove from the speed 
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
            // gradually  stop the vehicle
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

        // Only rotate if the player is moving forward
        /*if (forwardInput != 0)
        {
            transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);
        }*/
        if ( currentSpeed != 0)
        {
            transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);
        }
        

        transform.Translate(Vector3.forward * Time.deltaTime * currentSpeed);
    }
    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with an Object with tag : obstacle
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            //Debug.Log("Vehicle hit an obstacle!");
            Instantiate(explosionEffect, collision.contacts[0].point, Quaternion.identity);
            //Destroy(collision.gameObject);
            currentSpeed = 0;
            StartCoroutine(WaitAndLoadScene(explosionDuration));
        }
        Rigidbody obstacleRigidbody = collision.gameObject.GetComponent<Rigidbody>();
        /*if (obstacleRigidbody != null)
        {
            Vector3 forceDirection = collision.contacts[0].point - transform.position;
            forceDirection = -forceDirection.normalized;
            obstacleRigidbody.AddForce(forceDirection * collisionForce, ForceMode.Impulse);
            Debug.Log(forceDirection);
            Debug.Log(collisionForce);
            *//*obstacleRigidbody.AddForce(Vector3.up * upwardsForce, ForceMode.Impulse);
            Debug.Log("Force applied!!");*//*
        }*/
        if (obstacleRigidbody != null)
        {
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
    
}
