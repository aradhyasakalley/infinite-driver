/*using UnityEngine;

public class MagnetPickup : MonoBehaviour
{
    public float magnetDuration = 30f; // Duration for the magnet effect

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vehicle"))
        {
            other.GetComponent<PlayerController>().ActivateMagnet(magnetDuration);
            Destroy(gameObject); 
        }
    }
}*/
