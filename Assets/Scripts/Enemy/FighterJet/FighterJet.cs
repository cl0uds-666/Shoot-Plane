using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Slider healthBar; // Health bar slider
    [SerializeField] private Image fillImage; // Image component of the health bar fill area

    private int currentHealth;
    private float fireTimer; // Timer to track firing intervals
    private bool pullingUp = false; // Track if the jet is pulling up
    private bool hasPulledUp = false; // Track if the jet has reached the lower height and is eligible to level off

    private void Start()
    {
        // Initialize health
        currentHealth = maxHealth;

        // Set up health bar
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
            if (fillImage != null)
            {
                fillImage.color = Color.green; // Start with green
            }
        }

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

        // Auto-destroy the jet after a certain lifetime
        Destroy(gameObject, lifeTime);
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
        Vector3 upwardDirection = transform.forward + Vector3.up * 0.5f;
        upwardDirection.Normalize();
        Quaternion pullUpRotation = Quaternion.LookRotation(upwardDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, pullUpRotation, rotationSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * pullUpSpeed * Time.deltaTime);

        if (transform.position.y >= upperHeight.y)
        {
            hasPulledUp = true;
        }
    }

    private void LevelOff()
    {
        Vector3 forwardDirection = transform.forward;
        forwardDirection.y = 0;
        Quaternion levelOffRotation = Quaternion.LookRotation(forwardDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, levelOffRotation, rotationSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
            UpdateHealthBarColor();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBarColor()
    {
        if (fillImage != null)
        {
            float healthPercentage = (float)currentHealth / maxHealth;

            if (healthPercentage > 0.5f)
            {
                fillImage.color = Color.green;
            }
            else if (healthPercentage > 0.2f)
            {
                fillImage.color = Color.yellow;
            }
            else
            {
                fillImage.color = Color.red;
            }
        }
    }

    private void Die()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Enable gravity
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Disable kinematic to allow physics
            rb.useGravity = true;   // Enable gravity
        }

        if (healthBar != null)
        {
            Destroy(healthBar.gameObject);
        }

        Destroy(gameObject, 5f); // Destroy the plane after 5 seconds
    }

}
