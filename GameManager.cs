using UnityEngine;
 class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject vehicleObject;

    public Transform parent;

    void Start()
    {
        instance = this;
        // Change the vehicle type to Bus when the game starts

        Instantiate(vehicleObject, Vector3.zero, Quaternion.identity, parent);
    }
}



