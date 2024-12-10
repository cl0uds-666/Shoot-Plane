using UnityEngine;

public class EnemyPlane : MonoBehaviour
{
    public float speed = 10f; // Plane movement speed
    public GameObject bombPrefab; // Prefab for bombs
    public Transform bombDropPoint; // Drop point for bombs
    public float bombDropInterval = 2f; // Time between bomb drops
    private float dropTimer;

    private Transform player; // Reference to the player
    private Vector3 targetPosition; // Where the plane is flying
    private bool hasAttacked = false; // Track if the plane has attacked

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        // Set an initial target near the player
        if (player != null)
        {
            targetPosition = GetRandomPositionNearPlayer();
        }
        else
        {
            Debug.LogWarning("Player not found!");
        }
    }

    void Update()
    {
        // Move toward the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the plane has reached the target position
        if (!hasAttacked && Vector3.Distance(transform.position, targetPosition) < 1f)
        {
            hasAttacked = true;
            StartCoroutine(AttackAndExit());
        }

        // Bomb dropping logic during the attack phase
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

    Vector3 GetRandomPositionNearPlayer()
    {
        float range = 5f; // How far from the player the plane should aim
        Vector3 offset = new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
        return player.position + offset;
    }

    System.Collections.IEnumerator AttackAndExit()
    {
        // Simulate attacking phase
        yield return new WaitForSeconds(3f); // Attack duration

        // Set a new target to fly off the map
        targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 50f);
    }
}


