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

    private Rigidbody rb;
    private float moveInput;
    private float turnInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
}
