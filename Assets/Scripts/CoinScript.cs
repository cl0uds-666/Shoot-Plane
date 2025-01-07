using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] private int coinValue = 1; // The value of the coin

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{gameObject.name} triggered by: {other.name}");

        if (other.CompareTag("Player")) // Check if the player collects the coin
        {
            if (PlayerCurrency.Instance != null)
            {
                Debug.Log($"Coin collected by player! Value: {coinValue}");
                PlayerCurrency.Instance.AddCurrency(coinValue); // Add currency to the player
            }
            else
            {
                Debug.LogError("PlayerCurrency.Instance is null. Ensure PlayerCurrency is set up correctly.");
            }

            Destroy(gameObject); // Destroy the coin
        }
        else
        {
            Debug.Log($"{gameObject.name} triggered by non-player object: {other.tag}");
        }
    }
}
