using UnityEngine;

public class EnemyPlane : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float speed = 10f; // Movement speed
    [SerializeField] private float rotationSpeed = 2f; // Speed at which the plane rotates
    [SerializeField] private GameObject bombPrefab; // Prefab for bombs
    [SerializeField] private Transform bombDropPoint; // Drop point for bombs
    [SerializeField] private float bombDropInterval = 2f; // Time between bomb drops
    [SerializeField] private int maxAttackCycles = 3; // Number of times the plane can attack before leaving permanently
    [SerializeField] private float loopDelay = 2f; // Time to wait before turning around
    [SerializeField] private float ascentSpeed = 5f; // Speed of ascent
    [SerializeField] private float noseUpAngle = 15f; // Angle for nose-up during ascent
    [SerializeField] private float exitDistance = 200f; // Distance to travel forward before being destroyed

    private Transform player; // Reference to the player
    private Vector3 flyOverTarget; // Target position above the player
    private int attackCyclesCompleted = 0; // Counter for attack cycles
    private bool isReturning = false; // Whether the plane is returning to attack again
    private bool isAscending = false; // Whether the plane is ascending to Y=46
    private bool isExiting = false; // Whether the plane is exiting permanently
    private Quaternion returnRotation; // Rotation for returning to attack
    private Vector3 exitTarget; // Target position for the exit

    private float dropTimer;

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        UpdateFlyOverTarget(); // Initialize the first target
    }

    void Update()
    {
        if (isExiting)
        {
            HandleExit();
            return;
        }

        if (isAscending)
        {
            HandleAscending();
        }
        else if (!isReturning)
        {
            // Attack behavior
            UpdateFlyOverTarget(); // Update the target dynamically
            Vector3 directionToTarget = flyOverTarget - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            HandleBombDropping();
        }
        else
        {
            // Returning behavior
            transform.rotation = Quaternion.Slerp(transform.rotation, returnRotation, rotationSpeed * Time.deltaTime);
        }

        // Move forward in the current direction
        if (!isAscending)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void HandleBombDropping()
    {
        if (Vector3.Distance(transform.position, flyOverTarget) < 1f)
        {
            DropBomb();
            if (attackCyclesCompleted < maxAttackCycles)
            {
                attackCyclesCompleted++;
                isAscending = true; // Start ascending after the bomb drop
            }
            else
            {
                ExitScene();
            }
        }
    }

    private void DropBomb()
    {
        if (bombPrefab != null && bombDropPoint != null)
        {
            Instantiate(bombPrefab, bombDropPoint.position, bombDropPoint.rotation);
        }
    }

    private void UpdateFlyOverTarget()
    {
        if (player != null)
        {
            float heightAbovePlayer = 20f;
            flyOverTarget = new Vector3(player.position.x, player.position.y + heightAbovePlayer, player.position.z);
        }
    }

    private void HandleAscending()
    {
        // Gradually ascend to Y=46 while moving forward
        Vector3 ascentTarget = transform.position + transform.forward * speed * Time.deltaTime;
        ascentTarget.y = Mathf.MoveTowards(transform.position.y, 46f, ascentSpeed * Time.deltaTime);

        transform.position = ascentTarget;

        // Adjust the plane's pitch for a nose-up orientation
        Vector3 forwardDirection = transform.forward;
        forwardDirection.y = 0.3f; // Slight upward tilt
        Quaternion noseUpRotation = Quaternion.LookRotation(forwardDirection.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, noseUpRotation, rotationSpeed * Time.deltaTime);

        // Check if the plane has reached the desired height
        if (Mathf.Abs(transform.position.y - 46f) < 0.1f)
        {
            isAscending = false; // Stop ascending
            StartCoroutine(ReturnToAttack());
        }
    }

    private System.Collections.IEnumerator ReturnToAttack()
    {
        isReturning = true;
        yield return new WaitForSeconds(loopDelay);

        if (player != null)
        {
            UpdateFlyOverTarget(); // Ensure the target is updated
            returnRotation = Quaternion.LookRotation(flyOverTarget - transform.position);
        }
        isReturning = false;
    }

    private void ExitScene()
    {
        isExiting = true;

        // Set the exit target position far ahead of the plane
        exitTarget = transform.position + transform.forward * exitDistance;
        exitTarget.y = 46f; // Maintain height at Y=46
    }

    private void HandleExit()
    {
        // Move toward the exit target
        transform.position = Vector3.MoveTowards(transform.position, exitTarget, speed * Time.deltaTime);

        // Check if the plane has reached the exit target
        if (Vector3.Distance(transform.position, exitTarget) < 1f)
        {
            Destroy(gameObject); // Destroy the plane
        }
    }
}
