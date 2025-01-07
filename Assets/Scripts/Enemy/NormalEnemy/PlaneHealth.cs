using UnityEngine;
using UnityEngine.UI;

public class PlaneHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100; // Maximum health of the plane
    [SerializeField] private GameObject fireEffect; // Fire effect prefab
    [SerializeField] private GameObject smokeEffect; // Smoke effect prefab
    [SerializeField] private float destroyDelay = 5f; // Time before the plane is destroyed after death
    [SerializeField] private Slider healthBar; // Reference to the health bar slider
    [SerializeField] private Image fillImage; // Image component of the health bar fill area
    [SerializeField] private GameObject coinPrefab; // Prefab for the coin
    [SerializeField] private int numberOfCoins = 3; // Number of coins to drop
    [SerializeField] private int coinValue = 1; // Value of each coin (optional for currency system)

    private int currentHealth;
    private Rigidbody rb; // Rigidbody of the plane
    private bool isDead = false; // Track if the plane is already dead

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    void Start()
    {
        // Initialize current health
        currentHealth = maxHealth;

        // Initialize health bar
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;

            // Set initial color to green
            if (fillImage != null)
            {
                fillImage.color = Color.green;
            }
            else
            {
                Debug.LogWarning("Fill Image is not assigned in PlaneHealth script!");
            }
        }
        else
        {
            Debug.LogWarning("Health Bar is not assigned in PlaneHealth script!");
        }

        // Cache the Rigidbody component
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody attached to the plane!");
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"Plane hit! Current health: {currentHealth}/{maxHealth}");

        // Update the health bar
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

            Debug.Log($"Updating Health Bar Color: {healthPercentage * 100}% Health");

            if (healthPercentage > 0.5f)
            {
                fillImage.color = Color.green; // High health
                Debug.Log("Health Bar Color: Green");
            }
            else if (healthPercentage > 0.2f)
            {
                fillImage.color = Color.yellow; // Medium health
                Debug.Log("Health Bar Color: Yellow");
            }
            else
            {
                fillImage.color = Color.red; // Low health
                Debug.Log("Health Bar Color: Red");
            }
        }
        else
        {
            Debug.LogWarning("Fill Image not assigned in PlaneHealth script!");
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // Enable gravity
        if (rb != null)
        {
            rb.useGravity = true;
        }

        // Enable fire and smoke effects
        if (fireEffect != null)
        {
            fireEffect.SetActive(true);
        }
        if (smokeEffect != null)
        {
            smokeEffect.SetActive(true);
        }

        // Drop coins
        DropCoins();

        // Destroy the health bar
        if (healthBar != null)
        {
            Destroy(healthBar.gameObject);
        }

        // Schedule destruction of the plane after delay
        Destroy(gameObject, destroyDelay);

        Debug.Log("Plane destroyed! Falling with fire and smoke.");
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

        Debug.Log($"{numberOfCoins} coins dropped!");
    }
}
