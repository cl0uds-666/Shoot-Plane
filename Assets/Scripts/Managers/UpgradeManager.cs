using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private BulletHandler bulletHandler;

    [SerializeField] private int healthUpgradeCost = 50;
    [SerializeField] private int fireRateUpgradeCost = 75;
    [SerializeField] private int damageUpgradeCost = 100;

    [SerializeField] private int healthIncreaseAmount = 20;
    [SerializeField] private float fireRateDecreaseAmount = 0.1f;
    [SerializeField] private int damageIncreaseAmount = 5;

    public void UpgradeHealth()
    {
        if (CurrencyManager.Instance != null && CurrencyManager.Instance.SpendCurrency(healthUpgradeCost))
        {
            playerHealth.IncreaseMaxHealth(healthIncreaseAmount);
            Debug.Log("Health upgraded!");
        }
        else
        {
            Debug.LogWarning("Not enough currency to upgrade health!");
        }
    }

    public void UpgradeFireRate()
    {
        if (CurrencyManager.Instance != null && CurrencyManager.Instance.SpendCurrency(fireRateUpgradeCost))
        {
            bulletHandler.DecreaseFireRate(fireRateDecreaseAmount);
            Debug.Log("Fire rate upgraded!");
        }
        else
        {
            Debug.LogWarning("Not enough currency to upgrade fire rate!");
        }
    }

    public void UpgradeDamage()
    {
        if (CurrencyManager.Instance != null && CurrencyManager.Instance.SpendCurrency(damageUpgradeCost))
        {
            bulletHandler.IncreaseBulletDamage(damageIncreaseAmount);
            Debug.Log("Bullet damage upgraded!");
        }
        else
        {
            Debug.LogWarning("Not enough currency to upgrade damage!");
        }
    }
}
