using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpin : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float magnetSpeed = 100f; // Adjusted magnet speed for testing, adjust as needed

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
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, magnetSpeed * Time.deltaTime);
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
