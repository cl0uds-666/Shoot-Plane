using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerSpin : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 90.0f; // Speed of the propeller rotation

    // Update is called once per frame
    void Update()
    {
        // Rotate the propeller around its Z-axis continuously
        transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);
    }
}
