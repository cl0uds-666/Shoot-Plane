using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private int damage = 10; // Damage dealt by the bullet
    [SerializeField] private float speed = 20f; // Speed of the bullet
    [SerializeField] private float lifetime = 5f; // Time before the bullet is destroyed

    private Transform playerTransform; // Reference to the player's transform

    void Start()
    {
        // Find the player in the scene
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found!");
        }

        // Destroy the bullet after a certain time
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the bullet towards the player's last known position
        if (playerTransform != null)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the bullet hit the player
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // Apply damage to the player
            playerHealth.TakeDamage(damage);
        }

        // Destroy the bullet on impact
        Destroy(gameObject);
    }
}
