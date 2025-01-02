using UnityEngine;
using EZCameraShake;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 5f; // Radius of the explosion
    [SerializeField] private float explosionForce = 500f; // Force applied by the explosion
    [SerializeField] private int maxDamage = 50; // Maximum damage dealt to the player
    [SerializeField] private GameObject explosionEffect; // Explosion VFX prefab
    [SerializeField] private float lifetime = 5f; // Time before the bomb is destroyed


    private bool hasExploded = false;

    void Start()
    {
        // Destroy the bomb after a certain time
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Prevent multiple explosions
        if (hasExploded) return;
        hasExploded = true;

        // Trigger the explosion
        Explode();
    }

    void Explode()
    {
        // Instantiate explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Check for objects in the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            // Apply explosion force if the object has a Rigidbody
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            // Check if the object is the player
            PlayerHealth player = nearbyObject.GetComponent<PlayerHealth>();
            if (player != null)
            {
                CameraShaker.Instance.ShakeOnce(20f, 20f, .1f, 1f);

                // Calculate damage based on distance
                float distance = Vector3.Distance(transform.position, player.transform.position);
                float damageFactor = Mathf.Clamp01(1 - (distance / explosionRadius));
                int damage = Mathf.RoundToInt(maxDamage * damageFactor);
                player.TakeDamage(damage);
            }
        }

        // Destroy the bomb after exploding
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the explosion radius in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
