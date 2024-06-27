using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    public PlayerController playerController;

    public float maxSpeed = 260.0f;
    public float minSpeedArrowAngle = 0f;  
    public float maxSpeedArrowAngle = 180f; 

    [Header("UI")]
    public Text speedLabel;
    public RectTransform arrow;

    private float smoothSpeed = 0.0f;
    public float smoothingFactor = 0.1f; 

    private void Update()
    {
        float speed = playerController.currentSpeed * 3.6f;
        smoothSpeed = Mathf.Lerp(smoothSpeed, speed, smoothingFactor);
        smoothSpeed = Mathf.Clamp(smoothSpeed, 0, maxSpeed);
        if (speedLabel != null)
            speedLabel.text = ((int)smoothSpeed) + " km/h";
        if (arrow != null)
            arrow.localEulerAngles = new Vector3(0, 0, -Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, smoothSpeed / maxSpeed));
    }
}
