using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 20; // Damage dealt by the bullet
    [SerializeField] private float lifetime = 5f; // Time before the bullet is destroyed


    void Start()
    {
        // Destroy the bullet after a certain time
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the bullet hit a plane
        PlaneHealth planeHealth = collision.gameObject.GetComponent<PlaneHealth>();
        if (planeHealth != null)
        {
            // Apply damage to the plane
            planeHealth.TakeDamage(damage);
        }

        // Destroy the bullet on impact
        Destroy(gameObject);
    }
}
