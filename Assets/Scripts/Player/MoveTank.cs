using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTank : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f; // Speed of forward movement
    [SerializeField] private float rotateSpeed = 120.0f; // Speed of tank rotation
    [SerializeField] private GameObject[] leftWheels;
    [SerializeField] private GameObject[] rightWheels;
    [SerializeField] private float wheelRotateSpeed = 200.0f;
    [SerializeField] private Transform turretTransform; // Reference to the turret for movement alignment
    [SerializeField] private float uprightForce = 10f; // Force to keep the tank upright
    [SerializeField] private float tiltThreshold = 25f; // Maximum tilt angle before forced correction
    [SerializeField] private float correctionSpeed = 2f; // Speed at which the tank resets to upright

    private Rigidbody rb;
    private float moveInput;
    private float turnInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing from the tank!");
        }
    }

    void Update()
    {
        // Get input for movement and rotation
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        // Rotate wheels based on movement and rotation input
        RotateWheels(moveInput, turnInput);
    }

    void FixedUpdate()
    {
        // Move the tank in the direction the turret is facing
        MoveTankObj(moveInput);

        // Rotate the tank
        RotateTankObj(turnInput);

        // Enforce upright orientation
        EnforceUpright();
    }

    void MoveTankObj(float input)
    {
        if (turretTransform == null)
        {
            Debug.LogWarning("Turret Transform is not assigned!");
            return;
        }

        // Calculate movement direction based on turret's forward vector
        Vector3 moveDirection = turretTransform.forward * input * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDirection);
    }

    void RotateTankObj(float input)
    {
        // Rotate the tank using input
        float rotation = input * rotateSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0, rotation, 0);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    void RotateWheels(float moveInput, float rotateInput)
    {
        float wheelRotation = moveInput * wheelRotateSpeed * Time.fixedDeltaTime;

        // Rotate left wheels
        foreach (GameObject wheel in leftWheels)
        {
            if (wheel != null)
            {
                wheel.transform.Rotate(wheelRotation - rotateInput * wheelRotateSpeed * Time.deltaTime, 0.0f, 0.0f);
            }
        }

        // Rotate right wheels
        foreach (GameObject wheel in rightWheels)
        {
            if (wheel != null)
            {
                wheel.transform.Rotate(wheelRotation + rotateInput * wheelRotateSpeed * Time.deltaTime, 0.0f, 0.0f);
            }
        }
    }

    void EnforceUpright()
    {
        // Check tilt angle
        float tiltAngle = Vector3.Angle(Vector3.up, transform.up);

        if (tiltAngle > tiltThreshold)
        {
            // If the tilt exceeds the threshold, smoothly reset rotation to upright
            Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, correctionSpeed * Time.fixedDeltaTime);

            // dampen angular velocity to reduce wobble
            rb.angularVelocity = Vector3.zero;
        }
    }
}
