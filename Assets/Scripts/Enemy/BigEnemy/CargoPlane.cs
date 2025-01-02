using UnityEngine;

public class CargoPlane : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Movement speed
    [SerializeField] private int maxHealth = 200; // Maximum health
    [SerializeField] private GameObject bombPrefab; // Prefab for the bombs
    [SerializeField] private Transform[] bombDropPoints; // Positions where bombs are dropped
    [SerializeField] private float bombDropInterval = 0.5f; // Time between bomb drops
    [SerializeField] private GameObject explosionEffect; // Explosion VFX prefab
    [SerializeField] private float lifeTime = 20f; // Time before the plane is destroyed
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        InvokeRepeating(nameof(DropBombs), 1f, bombDropInterval);
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

        if (currentHealth <= 0)
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

        // Destroy the cargo plane
        Destroy(gameObject);
    }
}
