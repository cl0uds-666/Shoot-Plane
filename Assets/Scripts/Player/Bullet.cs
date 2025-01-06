using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 10; // Damage dealt by the bullet
    [SerializeField] private float lifetime = 5f; // Time before the bullet is destroyed

    void Start()
    {
        // Destroy the bullet after a certain time
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the bullet hit an enemy with PlaneHealth
        PlaneHealth planeHealth = other.GetComponent<PlaneHealth>();
        if (planeHealth != null)
        {
            planeHealth.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // Check if the bullet hit a FighterJet
        FighterJet fighterJet = other.GetComponent<FighterJet>();
        if (fighterJet != null)
        {
            fighterJet.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // Check if the bullet hit a CargoPlane
        CargoPlane cargoPlane = other.GetComponent<CargoPlane>();
        if (cargoPlane != null)
        {
            cargoPlane.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // If no valid target was hit, destroy the bullet
        Destroy(gameObject);
    }
}
