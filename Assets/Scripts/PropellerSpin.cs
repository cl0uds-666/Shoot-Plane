using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerSpin : MonoBehaviour
{
    public float spinSpeed = 360.0f;  // Speed of the propeller rotation (degrees per second)

    // Update is called once per frame
    void Update()
    {
        // Rotate the propeller around its forward axis (Z-axis) continuously
        transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);
    }
}
