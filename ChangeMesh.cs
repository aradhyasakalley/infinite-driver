using UnityEngine;

public class ChangeMesh : MonoBehaviour
{
    // Reference to the MeshFilter component of the vehicle
    private MeshFilter meshFilter;

    // Different meshes for each type of vehicle
    public Mesh busMesh;
    public Mesh tankMesh;
    public Mesh carMesh;
    public Mesh vanMesh;

    public enum VehicleType
    {
        Bus,
        Tank,
        Car,
        Van
    }

    public VehicleType vehicleType;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        // Retrieve the selected vehicle type from PlayerPrefs
        string selectedVehicle = PlayerPrefs.GetString("SelectedVehicle", "Car"); 

        switch (selectedVehicle)
        {
            case "Car":
                vehicleType = VehicleType.Car;
                break;
            case "Bus":
                vehicleType = VehicleType.Bus;
                break;
            case "Tank":
                vehicleType = VehicleType.Tank;
                break;
            case "Van":
                vehicleType = VehicleType.Van;
                break;
            default:
                Debug.LogError("Unknown vehicle type selected: " + selectedVehicle);
                break;
        }

        SetVehicleMesh();
    }

    private void SetVehicleMesh()
    {
        switch (vehicleType)
        {
            case VehicleType.Bus:
                meshFilter.mesh = busMesh;
                break;
            case VehicleType.Tank:
                meshFilter.mesh = tankMesh;
                break;
            case VehicleType.Car:
                meshFilter.mesh = carMesh;
                break;
            case VehicleType.Van:
                meshFilter.mesh = vanMesh;
                break;
            default:
                Debug.LogError("Unknown vehicle type.");
                break;
        }
    }

    // Function to change the vehicle type
    public void ChangeVehicleType(VehicleType newVehicleType)
    {
        vehicleType = newVehicleType;
        SetVehicleMesh();
    }
}
