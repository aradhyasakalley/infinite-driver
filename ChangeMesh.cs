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

    // Vehicle properties
    public float maxSpeed;
    public float acceleration;
    public float deceleration; 
    public float turnSpeed;

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
                SetCarProperties();
                break;
            case "Bus":
                vehicleType = VehicleType.Bus;
                SetBusProperties();
                break;
            case "Tank":
                vehicleType = VehicleType.Tank;
                SetTankProperties();
                break;
            case "Van":
                vehicleType = VehicleType.Van;
                SetVanProperties();
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

        // Update properties when vehicle type changes
        switch (vehicleType)
        {
            case VehicleType.Bus:
                SetBusProperties();
                break;
            case VehicleType.Tank:
                SetTankProperties();
                break;
            case VehicleType.Car:
                SetCarProperties();
                break;
            case VehicleType.Van:
                SetVanProperties();
                break;
            default:
                Debug.LogError("Unknown vehicle type.");
                break;
        }
    }

    // Define properties for each vehicle type
    private void SetCarProperties()
    {
        maxSpeed = 60f;
        acceleration = 5f;
        deceleration = 10f; 
        turnSpeed = 45f;
    }

    private void SetBusProperties()
    {
        maxSpeed = 70f;
        acceleration = 3f;
        deceleration = 10f;
        turnSpeed = 25f;
    }

    private void SetTankProperties()
    {
        maxSpeed = 40f;
        acceleration = 5f;
        deceleration = 10f;
        turnSpeed = 35f;
    }

    private void SetVanProperties()
    {
        maxSpeed = 60f;
        acceleration = 3f;
        deceleration = 10f;
        turnSpeed = 35f;
    }
}
