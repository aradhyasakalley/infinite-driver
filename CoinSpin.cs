using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpin : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float magnetStrength = 1000f; // Increased strength

    private bool isMagnetActive = false;
    private Transform playerTransform;

    void Start()
    {
        // Initially try to find the player transform
        AssignPlayerTransform();
    }

    void Update()
    {
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);

        if (playerTransform == null)
        {
            // Try to find the player transform in each frame until found
            AssignPlayerTransform();
        }

        if (isMagnetActive && playerTransform != null)
        {
            Debug.Log("Magnet is active");
            AttractTowardsPlayer();
        }
    }

    void AssignPlayerTransform()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Vehicle");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void AttractTowardsPlayer()
    {
        if (playerTransform != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            Rigidbody rb = GetComponent<Rigidbody>();

            if (rb != null)
            {
                Debug.Log("Applying force to the coin");
                rb.AddForce(direction * magnetStrength * Time.deltaTime, ForceMode.Force);
                Debug.Log("Force applied: " + (direction * magnetStrength * Time.deltaTime));
            }
            else
            {
                Debug.LogError("Rigidbody component is missing on the coin.");
            }
        }
    }

    public void ActivateMagnet(float duration)
    {
        isMagnetActive = true;
        StartCoroutine(DeactivateMagnetAfterTime(duration));
    }

    private IEnumerator DeactivateMagnetAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        isMagnetActive = false;
    }
}
