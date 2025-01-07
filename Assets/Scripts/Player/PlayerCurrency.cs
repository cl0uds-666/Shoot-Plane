using TMPro; // For TextMeshPro
using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    public static PlayerCurrency Instance;

    private int currentCurrency = 0;

    // Reference to the TextMeshPro UI element for displaying coins
    [SerializeField] private TextMeshProUGUI currencyDisplay;

    public delegate void CurrencyChanged(int newCurrency);
    public static event CurrencyChanged OnCurrencyChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateCurrencyDisplay(); // Initialize the display with the current value
    }

    public void AddCurrency(int amount)
    {
        currentCurrency += amount;
        Debug.Log($"Currency Added: {amount}. Total Currency: {currentCurrency}");
        UpdateCurrencyDisplay();

        OnCurrencyChanged?.Invoke(currentCurrency);
    }

    public int GetCoins()
    {
        return currentCurrency;
    }

    public void SpendCoins(int amount)
    {
        if (currentCurrency >= amount)
        {
            currentCurrency -= amount;
            Debug.Log($"Currency Spent: {amount}. Remaining Currency: {currentCurrency}");
            UpdateCurrencyDisplay();

            OnCurrencyChanged?.Invoke(currentCurrency);
        }
        else
        {
            Debug.LogWarning("Not enough currency to complete the transaction.");
        }
    }

    public void AddTestCoins(int amount)
    {
        AddCurrency(amount);
        Debug.Log($"Test: Added {amount} coins. Total coins: {GetCoins()}");
    }

    private void UpdateCurrencyDisplay()
    {
        if (currencyDisplay != null)
        {
            currencyDisplay.text = $"Coins: {currentCurrency}";
        }
        else
        {
            Debug.LogWarning("Currency Display is not assigned in the Inspector.");
        }
    }

    public void ResetCurrency(int amount)
    {
        currentCurrency = amount;
        UpdateCurrencyDisplay();
        Debug.Log($"Currency reset. New total: {currentCurrency}");
    }

}
