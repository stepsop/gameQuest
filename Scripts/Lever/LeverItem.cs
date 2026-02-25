using UnityEngine;

public class LeverItem : MonoBehaviour, IInteractable
{
    private bool playerInside;
    public ItemData itemData;

    public bool CanInteract()
    {
        return playerInside;
    }

    public void Interact()
    {
        InventoryManager.Instance.AddItem(itemData);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("STEP 6.7: Игрок рядом с предметом");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            Debug.Log("STEP 6.7: Игрок отошёл от предмета");
        }
    }
}
