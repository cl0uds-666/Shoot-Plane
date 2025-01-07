using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText; // Reference for the player's health text
    [SerializeField] private TextMeshProUGUI coinText; // Reference for the player's coin text

    void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateHealthUI;
        PlayerCurrency.OnCurrencyChanged += UpdateCoinUI;
    }

    void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateHealthUI;
        PlayerCurrency.OnCurrencyChanged -= UpdateCoinUI;
    }

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {currentHealth}/{maxHealth}";
        }
    }

    public void UpdateCoinUI(int currentCoins)
    {
        if (coinText != null)
        {
            coinText.text = $"Coins: {currentCoins}";
        }
    }
}
