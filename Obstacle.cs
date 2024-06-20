using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collided with obstacle!");

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
                rb.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);
                rb.useGravity = true;
                rb.constraints = RigidbodyConstraints.None;
            }
        }
    }
}

