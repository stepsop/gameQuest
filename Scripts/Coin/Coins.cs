using UnityEngine;

public class Coin : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData coinItemData; // 📦 Ссылка на ассет монеты
    [SerializeField] private int coinAmount = 1;    // Количество монет в одной "кучке"

    private bool playerInside;

    public bool CanInteract()
    {
        return playerInside; // Можно подобрать, если игрок рядом
    }

    public void Interact()
    {
        if (!CanInteract()) return;

        // Добавляем монету в инвентарь (метод AddItem мы изменим ниже)
        InventoryManager.Instance.AddItem(coinItemData, coinAmount);
        
        // Уничтожаем объект монеты на сцене
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("Монета: игрок рядом, можно подобрать через E");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}