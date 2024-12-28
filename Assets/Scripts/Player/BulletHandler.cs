using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    [SerializeField] private float launchSpeed = 75.0f; // Speed at which the bullet is launched
    [SerializeField] private GameObject objectPrefab; // Prefab for the bullet
    [SerializeField] private float lifetime = 5f; // Time before the bullet is destroyed

    void Update()
    {
        // Fire bullet when Mouse Button 1 (left mouse button) is pressed
        if (Input.GetMouseButtonDown(0)) // 0 corresponds to the left mouse button
        {
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        // Get the turret's current position
        Vector3 spawnPosition = transform.position;

        // Get the prefab's rotation (keeping X and Z)
        Quaternion prefabRotation = objectPrefab.transform.rotation;

        // Get the current Y rotation of the turret
        float turretYRotation = transform.eulerAngles.y;

        // Define the Y-axis correction factor (difference between 328 and 235 degrees)
        float yRotationCorrection = 93.0f;

        // Adjust the turret's Y rotation with the correction factor
        float adjustedYRotation = turretYRotation - yRotationCorrection;

        // Construct a new rotation that uses the adjusted Y rotation, and the prefab's X and Z rotation
        Quaternion spawnRotation = Quaternion.Euler(prefabRotation.eulerAngles.x, adjustedYRotation, prefabRotation.eulerAngles.z);

        // Calculate velocity in the forward direction of the turret
        Vector3 fireDirection = transform.forward;
        Vector3 velocity = fireDirection * launchSpeed;

        // Instantiate the bullet with the new adjusted rotation
        GameObject newBullet = Instantiate(objectPrefab, spawnPosition, spawnRotation);

        // Apply velocity to the bullet's Rigidbody
        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        rb.velocity = velocity;

        // Destroy the bullet after the set lifetime
        Destroy(newBullet, lifetime);
    }
}
