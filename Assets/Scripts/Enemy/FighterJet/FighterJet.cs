using UnityEngine;

public class FighterJet : MonoBehaviour
{
    [SerializeField] private float speed = 15f; // Movement speed
    [SerializeField] private int maxHealth = 1; // Maximum health (1-shot kill)
    [SerializeField] private GameObject bulletPrefab; // Prefab for the bullets
    [SerializeField] private Transform gunPoint; // Position where bullets are fired
    [SerializeField] private float fireRate = 0.1f; // Time between bullet shots
    [SerializeField] private GameObject explosionEffect; // Explosion VFX prefab
    [SerializeField] private Transform playerTransform; // Reference to the player
    [SerializeField] private float rotationSpeed = 2f; // Speed of rotation towards the player
    [SerializeField] private Vector3 lowerHeight = new Vector3(93.25f, 12.39f, 54.69f); // Lower dive height
    [SerializeField] private Vector3 upperHeight = new Vector3(93.25f, 46.39f, 64.72f); // Level-off height
    [SerializeField] private float pullUpSpeed = 20f; // Speed during pull-up
    [SerializeField] private float lifeTime = 20f; // Time before the jet is destroyed

    private float fireTimer; // Timer to track firing intervals
    private bool pullingUp = false; // Track if the jet is pulling up
    private bool hasPulledUp = false; // Track if the jet has reached the lower height and is eligible to level off

    private void Start()
    {
        // Find the player in the scene if not already assigned
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogWarning("Player not found!");
            }
        }
        Destroy(gameObject, lifeTime); // Destroy the jet after a certain time (if not already destroyed)
    }

    private void Update()
    {
        if (!pullingUp)
        {
            // Normal behavior: rotate to face the player and fire bullets
            if (playerTransform != null)
            {
                Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Move forward in the current direction
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            // Fire bullets continuously
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                FireBullets();
                fireTimer = 0f;
            }

            // Check if the jet is too low
            if (transform.position.y <= lowerHeight.y)
            {
                StartPullUp();
            }
        }
        else
        {
            // Pull up and level off
            if (!hasPulledUp)
            {
                PullUp();
            }
            else
            {
                LevelOff();
            }
        }
    }

    private void FireBullets()
    {
        if (gunPoint != null && bulletPrefab != null)
        {
            Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
        }
    }

    private void StartPullUp()
    {
        pullingUp = true;
    }

    private void PullUp()
    {
        // Adjust the direction to pull up
        Vector3 upwardDirection = transform.forward + Vector3.up * 0.5f;
        upwardDirection.Normalize();
        Quaternion pullUpRotation = Quaternion.LookRotation(upwardDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, pullUpRotation, rotationSpeed * Time.deltaTime);

        // Move forward while pulling up
        transform.Translate(Vector3.forward * pullUpSpeed * Time.deltaTime);

        // Check if it has reached the upper height
        if (transform.position.y >= upperHeight.y)
        {
            hasPulledUp = true;
        }
    }

    private void LevelOff()
    {
        // Smoothly level off and stop climbing
        Vector3 forwardDirection = transform.forward;
        forwardDirection.y = 0; // Remove any upward pitch
        Quaternion levelOffRotation = Quaternion.LookRotation(forwardDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, levelOffRotation, rotationSpeed * Time.deltaTime);

        // Move forward at normal speed
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        maxHealth -= damage;

        if (maxHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Trigger explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Destroy the fighter jet
        Destroy(gameObject);
    }
}
