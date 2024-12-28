using UnityEngine;

public class PlaneHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100; // Maximum health of the plane
    [SerializeField] private GameObject fireEffect; // Fire effect prefab
    [SerializeField] private GameObject smokeEffect; // Smoke effect prefab
    [SerializeField] private float destroyDelay = 5f; // Time before the plane is destroyed after death

    private int currentHealth;
    private Rigidbody rb; // Rigidbody of the plane
    private bool isDead = false; // Track if the plane is already dead

    void Start()
    {
        // Initialize current health
        currentHealth = maxHealth;

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
        Debug.Log($"Plane hit! Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
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

        // Schedule destruction of the plane after delay
        Destroy(gameObject, destroyDelay);

        Debug.Log("Plane destroyed! Falling with fire and smoke.");
    }
}
