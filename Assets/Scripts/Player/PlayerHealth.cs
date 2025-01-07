using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    // Event to notify UI or other systems about health changes
    public delegate void HealthChanged(int currentHealth, int maxHealth);
    public static event HealthChanged OnHealthChanged;

    void Start()
    {
        currentHealth = maxHealth;

        // Notify listeners about the initial health state
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");

        // Notify listeners about the health change
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
        FindObjectOfType<GameManager>().PlayerDied();
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth; // Optionally refill health
        Debug.Log($"Max Health Increased. New Max Health: {maxHealth}");
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void ResetHealth(int maxHealthValue)
    {
        maxHealth = maxHealthValue;
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log($"Health reset. Max Health: {maxHealth}, Current Health: {currentHealth}");
    }

}
