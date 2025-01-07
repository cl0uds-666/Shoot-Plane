using UnityEngine;
using UnityEngine.UI;

public class CargoPlane : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Movement speed
    [SerializeField] private int maxHealth = 200; // Maximum health
    [SerializeField] private GameObject bombPrefab; // Prefab for the bombs
    [SerializeField] private Transform[] bombDropPoints; // Positions where bombs are dropped
    [SerializeField] private float bombDropInterval = 0.5f; // Time between bomb drops
    [SerializeField] private GameObject explosionEffect; // Explosion VFX prefab
    [SerializeField] private float lifeTime = 20f; // Time before the plane is destroyed
    [SerializeField] private Slider healthBar; // Health bar slider
    [SerializeField] private Image fillImage; // Image component of the health bar fill area
    [SerializeField] private GameObject coinPrefab; // Prefab for the coin
    [SerializeField] private int numberOfCoins = 10; // Number of coins to drop

    private int currentHealth;

    private void Start()
    {
        // Initialize health
        currentHealth = maxHealth;

        // Set up health bar
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;

            // Set initial color to green
            if (fillImage != null)
            {
                fillImage.color = Color.green;
            }
        }

        // Start dropping bombs
        InvokeRepeating(nameof(DropBombs), 1f, bombDropInterval);

        // Auto-destroy after lifetime
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // Move forward continuously
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void DropBombs()
    {
        foreach (Transform dropPoint in bombDropPoints)
        {
            Instantiate(bombPrefab, dropPoint.position, dropPoint.rotation);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Update health bar
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
                fillImage.color = Color.green; // High health
            }
            else if (healthPercentage > 0.2f)
            {
                fillImage.color = Color.yellow; // Medium health
            }
            else
            {
                fillImage.color = Color.red; // Low health
            }
        }
    }

    private void Die()
    {
        // Notify GameManager of the kill
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.IncrementKillCount();
        }
        else
        {
            Debug.LogWarning("GameManager not found! Kill count will not be tracked.");
        }

        // Trigger explosion effect
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

        // Drop coins
        DropCoins();

        // Destroy the health bar
        if (healthBar != null)
        {
            Destroy(healthBar.gameObject);
        }

        Destroy(gameObject, 5f); // Destroy the plane after 5 seconds
    }


    private void DropCoins()
    {
        if (coinPrefab == null) return;

        for (int i = 0; i < numberOfCoins; i++)
        {
            // Randomize coin drop position slightly around the enemy's position
            Vector3 dropPosition = transform.position + new Vector3(
                Random.Range(-1f, 1f),
                0.5f,
                Random.Range(-1f, 1f)
            );

            Instantiate(coinPrefab, dropPosition, Quaternion.identity);
        }

        Debug.Log($"{numberOfCoins} coins dropped by Cargo Plane!");
    }
}
