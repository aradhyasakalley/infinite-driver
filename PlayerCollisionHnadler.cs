using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Handle collision with the obstacle
            Debug.Log("Player collided with obstacle!");

            // Apply a force to make the obstacle fly away
            Rigidbody obstacleRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (obstacleRigidbody != null)
            {
                Vector3 forceDirection = collision.contacts[0].point - transform.position;
                forceDirection = -forceDirection.normalized;
                obstacleRigidbody.AddForce(forceDirection * 10f, ForceMode.Impulse);
                obstacleRigidbody.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);
                obstacleRigidbody.useGravity = true;
                obstacleRigidbody.constraints = RigidbodyConstraints.None;
            }

            // Stop the game (you can replace this with your specific game over logic)
            Time.timeScale = 0f; // This pauses the game
        }
    }
}
