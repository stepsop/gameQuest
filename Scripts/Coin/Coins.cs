using UnityEngine;

public class Coin : MonoBehaviour
{
    private void Start()
    {
        if (CoinsCounterManager.Instance != null)
        {
            CoinsCounterManager.Instance.RegisterCoin();
        }
        else
        {
            Debug.LogError("CoinsCounterManager НЕ найден в сцене!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CoinsCounterManager.Instance.CollectCoin();
            Destroy(gameObject);
        }
    }
}
