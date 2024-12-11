using UnityEngine;

public class EnemyPlane : MonoBehaviour
{
    public float speed = 10f; // Plane movement speed
    public float rotationSpeed = 5f; // Speed at which the plane rotates
    public GameObject bombPrefab; // Prefab for bombs
    public Transform bombDropPoint; // Drop point for bombs
    public float bombDropInterval = 2f; // Time between bomb drops
    private float dropTimer;

    private Transform player; // Reference to the player
    private Vector3 flyOverTarget; // Target position above the player
    private bool hasAttacked = false; // Track if the plane has attacked
    private bool exiting = false; // Whether the plane is exiting the scene

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        if (player != null)
        {
            // Set an initial target position above the player
            flyOverTarget = GetPositionAbovePlayer();
        }
        else
        {
            Debug.LogWarning("Player not found!");
        }
    }

    void Update()
    {
        if (!exiting)
        {
            // Rotate to face the target position
            Vector3 directionToTarget = flyOverTarget - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Move forward in the current facing direction
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Bomb dropping logic
        if (!hasAttacked && Vector3.Distance(transform.position, flyOverTarget) < 1f)
        {
            hasAttacked = true;
            StartCoroutine(FlyOff());
        }

        if (!hasAttacked)
        {
            dropTimer += Time.deltaTime;
            if (dropTimer >= bombDropInterval)
            {
                DropBomb();
                dropTimer = 0f;
            }
        }
    }

    void DropBomb()
    {
        if (bombPrefab != null && bombDropPoint != null)
        {
            Instantiate(bombPrefab, bombDropPoint.position, bombDropPoint.rotation);
        }
    }

    Vector3 GetPositionAbovePlayer()
    {
        float heightAbovePlayer = 20f; // Height above the player
        return new Vector3(player.position.x, player.position.y + heightAbovePlayer, player.position.z);
    }

    System.Collections.IEnumerator FlyOff()
    {
        // Wait for a short period before exiting
        yield return new WaitForSeconds(2f);

        // Set the plane to exiting mode, so it keeps moving in the current direction
        exiting = true;
    }
}
