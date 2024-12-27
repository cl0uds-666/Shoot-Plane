using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTank : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float rotateSpeed = 120.0f;
    [SerializeField] private GameObject[] leftWheels;
    [SerializeField] private GameObject[] rightWheels;
    [SerializeField] private float wheelRotateSpeed = 200.0f;

    private Rigidbody rb;
    private float moveInput;
    private float turnInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();  
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
        RotateWheels(moveInput, turnInput);

    }

    void FixedUpdate()
    {
        MoveTankObj(moveInput);
        RotateTankObj(turnInput);
    }

    void MoveTankObj(float input)
    {
        Vector3 moveDirection = transform.forward * input * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDirection);
    }

    void RotateTankObj(float input)
    {
        float rotation = input * rotateSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0, rotation, 0);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    void RotateWheels(float moveInput, float rotateInput)
    {
        float wheelRotation = moveInput * wheelRotateSpeed * Time.fixedDeltaTime;
        
        // Left wheels
        foreach (GameObject wheel in leftWheels)
        {
            if (wheel != null)
            {
                wheel.transform.Rotate(wheelRotation - rotateInput * wheelRotateSpeed * Time.deltaTime, 0.0f, 0.0f);
            }
        }

        // Right wheels
        foreach (GameObject wheel in rightWheels)
        {
            if (wheel != null)
            {
                wheel.transform.Rotate(wheelRotation + rotateInput * wheelRotateSpeed * Time.deltaTime, 0.0f, 0.0f);
            }
        }


    }
}
