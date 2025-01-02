using UnityEngine;

public class EnemyPlane : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // Plane movement speed
    [SerializeField] private float rotationSpeed = 5f; // Speed at which the plane rotates
    [SerializeField] private GameObject bombPrefab; // Prefab for bombs
    [SerializeField] private Transform bombDropPoint; // Drop point for bombs
    [SerializeField] private float bombDropInterval = 2f; // Time between bomb drops
    [SerializeField] private float lifeTime = 20f; // Time before the plane gets destroyed

    private float dropTimer;
    private Transform player; // Reference to the player
    private Vector3 flyOverTarget; // Target position above the player
    private bool hasDroppedInitialBomb = false; // Track if the plane has dropped its mandatory bomb
    private bool exiting = false; // Whether the plane is exiting the scene
    private Quaternion exitRotation; // Smoothed-out exit direction

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        if (player != null)
        {
            flyOverTarget = GetPositionAbovePlayer();
        }
        else
        {
            Debug.LogWarning("Player not found!");
        }

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (!exiting)
        {
            // Rotate to face the target position
            Vector3 directionToTarget = flyOverTarget - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Check if it's time to drop bombs
            HandleBombDropping();
        }
        else
        {
            // Smoothly level off the plane during exit
            transform.rotation = Quaternion.Slerp(transform.rotation, exitRotation, rotationSpeed * Time.deltaTime);
        }

        // Move forward in the current facing direction
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void HandleBombDropping()
    {
        // Ensure at least one bomb is dropped when reaching the target
        if (!hasDroppedInitialBomb && Vector3.Distance(transform.position, flyOverTarget) < 1f)
        {
            DropBomb();
            hasDroppedInitialBomb = true;
            StartCoroutine(FlyOff());
        }

        // Continue dropping bombs at intervals
        if (hasDroppedInitialBomb)
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
        // Adjust the exit direction to level off
        Vector3 forwardDirection = transform.forward;
        forwardDirection.y = 0; // Remove any vertical pitch
        exitRotation = Quaternion.LookRotation(forwardDirection);

        // Set exiting mode to true
        exiting = true;

        yield break; // End coroutine
    }
}
