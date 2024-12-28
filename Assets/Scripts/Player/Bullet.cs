using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 20; // Damage dealt by the bullet

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
