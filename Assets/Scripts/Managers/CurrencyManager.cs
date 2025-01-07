using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    private int currentCurrency = 0;

    // Define the OnCurrencyChanged event
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

    public int GetCurrency()
    {
        return currentCurrency;
    }

    public void AddCurrency(int amount)
    {
        currentCurrency += amount;
        Debug.Log($"Currency Added: {amount}. Total: {currentCurrency}");

        // Trigger the event
        OnCurrencyChanged?.Invoke(currentCurrency);
    }

    public bool SpendCurrency(int amount)
    {
        if (currentCurrency >= amount)
        {
            currentCurrency -= amount;
            Debug.Log($"Currency Spent: {amount}. Remaining: {currentCurrency}");

            // Trigger the event
            OnCurrencyChanged?.Invoke(currentCurrency);
            return true;
        }
        Debug.LogWarning("Not enough currency!");
        return false;
    }

    public void ResetCurrency(int amount = 0)
    {
        currentCurrency = amount;
        Debug.Log($"Currency Reset. New Total: {currentCurrency}");

        // Trigger the event
        OnCurrencyChanged?.Invoke(currentCurrency);
    }
}
