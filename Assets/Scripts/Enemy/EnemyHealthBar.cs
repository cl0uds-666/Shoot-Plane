using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar; // Reference to the health bar slider
    [SerializeField] private PlaneHealth planeHealth; // Reference to the enemy's health script
    [SerializeField] private Vector3 offset = new Vector3(0, 2, 0); // Offset for the health bar position

    void Start()
    {
        // Set the maximum value of the health bar
        if (healthBar != null && planeHealth != null)
        {
            healthBar.maxValue = planeHealth.GetMaxHealth();
            healthBar.value = planeHealth.GetCurrentHealth();
        }
    }

    void Update()
    {
        // Update the health bar's value
        if (healthBar != null && planeHealth != null)
        {
            healthBar.value = planeHealth.GetCurrentHealth();

            // Position the health bar above the enemy
            healthBar.transform.position = transform.position + offset;
        }
    }
}
