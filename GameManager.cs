using UnityEngine;
 class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject vehicleObject;

    public Transform parent;

    void Start()
    {
        instance = this;
        Instantiate(vehicleObject, Vector3.zero, Quaternion.identity, parent);
    }
}



