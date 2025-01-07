using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth; // Reference to PlayerHealth script
    [SerializeField] private BulletHandler bulletHandler; // Reference to BulletHandler script
    [SerializeField] private PlayerCurrency playerCurrency; // Reference to PlayerCurrency script

    [SerializeField] private int healthUpgradeCost = 50;
    [SerializeField] private int fireRateUpgradeCost = 75;
    [SerializeField] private int damageUpgradeCost = 100;

    [SerializeField] private int healthIncreaseAmount = 20; // Amount of health per upgrade
    [SerializeField] private float fireRateDecreaseAmount = 0.1f; // Decrease in fire rate
    [SerializeField] private int damageIncreaseAmount = 5; // Amount of damage per upgrade

    public void UpgradeHealth()
    {
        if (playerCurrency.GetCoins() >= healthUpgradeCost)
        {
            playerCurrency.SpendCoins(healthUpgradeCost);
            playerHealth.IncreaseMaxHealth(healthIncreaseAmount);
            Debug.Log($"Health upgraded! New max health: {playerHealth.GetMaxHealth()}");
        }
        else
        {
            Debug.Log("Not enough coins to upgrade health!");
        }
    }

    public void UpgradeFireRate()
    {
        if (playerCurrency.GetCoins() >= fireRateUpgradeCost)
        {
            playerCurrency.SpendCoins(fireRateUpgradeCost);
            bulletHandler.DecreaseFireRate(fireRateDecreaseAmount);
            Debug.Log($"Fire rate upgraded! New fire rate: {bulletHandler.GetFireRate()}");
        }
        else
        {
            Debug.Log("Not enough coins to upgrade fire rate!");
        }
    }

    public void UpgradeDamage()
    {
        if (playerCurrency.GetCoins() >= damageUpgradeCost)
        {
            playerCurrency.SpendCoins(damageUpgradeCost);
            bulletHandler.IncreaseBulletDamage(damageIncreaseAmount);
            Debug.Log($"Bullet damage upgraded! New damage: {bulletHandler.GetBulletDamage()}");
        }
        else
        {
            Debug.Log("Not enough coins to upgrade damage!");
        }
    }
}
