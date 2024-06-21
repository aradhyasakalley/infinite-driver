using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject Vehicle;
    private Vector3 offset = new Vector3(0, 10, -20);

    void LateUpdate()
    {
        transform.position = Vehicle.transform.position + offset;
    }
}
