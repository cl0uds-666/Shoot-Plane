using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float tiltSpeed = 90.0f;  // Speed for rotating up and down
    private float currentTilt = 0.0f; // Track current tilt angle, relative to the starting rotation

    // Update is called once per frame
    void Update()
    {
        // Calculate tilt delta
        float tiltDelta = 0.0f;

        if (Input.GetKey(KeyCode.R)) // Rotate up
        {
            tiltDelta = -tiltSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.F)) // Rotate down
        {
            tiltDelta = tiltSpeed * Time.deltaTime;
        }

        // Calculate the new tilt and clamp it between -45 and +20 degrees
        float newTilt = Mathf.Clamp(currentTilt + tiltDelta, -45.0f, 20.0f);

        // Apply the tilt difference to the transform
        float tiltChange = newTilt - currentTilt;
        transform.Rotate(Vector3.right, tiltChange);

        // Update current tilt
        currentTilt = newTilt;
    }
}
