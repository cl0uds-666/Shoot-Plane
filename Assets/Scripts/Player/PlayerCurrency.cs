using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    public static PlayerCurrency Instance; // Singleton instance for easy access

    private int currentCurrency = 0;

    public delegate void CurrencyChanged(int newCurrency);
    public static event CurrencyChanged OnCurrencyChanged;

    private void Awake()
    {
        // Ensure only one instance of PlayerCurrency exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
            Debug.Log("PlayerCurrency initialized.");
        }
        else
        {
            Debug.LogWarning("Multiple PlayerCurrency instances detected. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    public void AddCurrency(int amount)
    {
        currentCurrency += amount;
        Debug.Log($"Currency Added: {amount}. Total Currency: {currentCurrency}");

        // Notify listeners about the currency change
        OnCurrencyChanged?.Invoke(currentCurrency);
    }

    public int GetCurrency()
    {
        Debug.Log($"Current Currency: {currentCurrency}");
        return currentCurrency;
    }
}
