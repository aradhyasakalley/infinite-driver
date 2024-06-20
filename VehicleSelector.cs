
using UnityEngine;
using UnityEngine.SceneManagement;

public class VehicleSelector : MonoBehaviour
{
    public void SelectVehicle(string vehicleType)
    {
        PlayerPrefs.SetString("SelectedVehicle", vehicleType);
        Debug.Log(vehicleType);
        SceneManager.LoadSceneAsync(2);
    }
}
