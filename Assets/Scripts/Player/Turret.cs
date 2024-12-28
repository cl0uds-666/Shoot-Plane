using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private float tiltSpeed = 90.0f;  // Speed for vertical rotation
    [SerializeField] private float tiltClampMin = -45f;  // Minimum tilt angle
    [SerializeField] private float tiltClampMax = 45f;   // Maximum tilt angle

    private float currentTilt = 0.0f; // Track the vertical tilt

    void Update()
    {
        // Use mouse Y-axis for vertical rotation
        float verticalRotation = -Input.GetAxis("Mouse Y") * tiltSpeed * Time.deltaTime; // Invert Y-axis for natural control
        float newTilt = Mathf.Clamp(currentTilt + verticalRotation, tiltClampMin, tiltClampMax);
        float tiltChange = newTilt - currentTilt;

        transform.Rotate(Vector3.right, tiltChange, Space.Self); // Apply local tilt rotation
        currentTilt = newTilt;
    }
}
