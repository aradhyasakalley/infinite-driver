using UnityEngine;

public class ChangeVehicleMesh : MonoBehaviour
{
    public Mesh newMesh; 
    private MeshFilter meshFilter;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            if (newMesh != null)
            {
                meshFilter.mesh = newMesh;
            }
            else
            {
                Debug.LogError("New mesh not assigned!");
            }
        }
        else
        {
            Debug.LogError("MeshFilter component not found!");
        }
    }

    public void ChangeMesh(Mesh newMesh)
    {
        if (meshFilter != null)
        {
            meshFilter.mesh = newMesh;
        }
        else
        {
            Debug.LogError("MeshFilter component not found!");
        }
    }
}

