using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    public void AddCurrency(int amount)
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddCurrency(amount);
        }
        else
        {
            Debug.LogError("CurrencyManager instance is missing!");
        }
    }

    public int GetCoins()
    {
        return CurrencyManager.Instance != null ? CurrencyManager.Instance.GetCurrency() : 0;
    }

    public bool SpendCoins(int amount)
    {
        return CurrencyManager.Instance != null && CurrencyManager.Instance.SpendCurrency(amount);
    }

    public void ResetCurrency(int amount)
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.ResetCurrency(amount);
        }
    }
}
