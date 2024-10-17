using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float spinSpeed = 90.0f;  // Speed for rotation left and right

    // Update is called once per frame
    void Update()
    {
        // Handle horizontal rotation (left and right)
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -spinSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
        }

    }
}
