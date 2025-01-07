using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (CurrencyManager.Instance != null)
            {
                CurrencyManager.Instance.AddCurrency(coinValue);
                Debug.Log($"Player collected coin! Value: {coinValue}");
            }
            else
            {
                Debug.LogError("CurrencyManager instance is missing!");
            }
            Destroy(gameObject);
        }
    }
}
