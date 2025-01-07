using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI coinText;

    void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateHealthUI;
        CurrencyManager.OnCurrencyChanged += UpdateCoinUI; // Subscribe to the event
    }

    void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateHealthUI;
        CurrencyManager.OnCurrencyChanged -= UpdateCoinUI; // Unsubscribe from the event
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
