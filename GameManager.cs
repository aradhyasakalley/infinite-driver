using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject carPrefab;
    public GameObject tankPrefab;

    void Start()
    {
        bool useCar = true; 

        if (useCar)
        {
            carPrefab.SetActive(true);
            tankPrefab.SetActive(false);
        }
        else
        {
            carPrefab.SetActive(false);
            tankPrefab.SetActive(true);
        }
    }
}
