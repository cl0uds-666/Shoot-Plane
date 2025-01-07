using UnityEngine;

public class ResetManager : MonoBehaviour
{
    [SerializeField] private PlayerCurrency playerCurrency; // Reference to PlayerCurrency script
    [SerializeField] private PlayerHealth playerHealth; // Reference to PlayerHealth script
    [SerializeField] private BulletHandler bulletHandler; // Reference to BulletHandler script

    // Original values
    [SerializeField] private int startingCoins = 0;
    [SerializeField] private int startingHealth = 100;
    [SerializeField] private int startingBulletDamage = 10;
    [SerializeField] private float startingFireRate = 0.2f;

    public void ResetAll()
    {
        // Reset currency
        if (playerCurrency != null)
        {
            playerCurrency.AddCurrency(-playerCurrency.GetCoins());
            playerCurrency.AddCurrency(startingCoins);
            Debug.Log("Currency reset to starting value.");
        }

        // Reset health
        if (playerHealth != null)
        {
            playerHealth.IncreaseMaxHealth(-playerHealth.GetMaxHealth() + startingHealth);
            Debug.Log("Health reset to starting value.");
        }

        // Reset bullet damage and fire rate
        if (bulletHandler != null)
        {
            bulletHandler.IncreaseBulletDamage(-bulletHandler.GetBulletDamage() + startingBulletDamage);
            bulletHandler.DecreaseFireRate(-bulletHandler.GetFireRate() + startingFireRate);
            Debug.Log("Bullet damage and fire rate reset to starting values.");
        }
    }
}
