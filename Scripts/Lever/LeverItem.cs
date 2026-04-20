using UnityEngine;

public class LeverItem : MonoBehaviour, IInteractable
{
    private bool playerInside;
    public ItemData itemData;

    // Уникальный ID этого предмета в этой сцене
    private string itemID;

    private void Start()
    {
        itemID = gameObject.scene.name + "_" + gameObject.name;

        if (PickupTracker.Instance != null && PickupTracker.Instance.IsPickedUp(itemID))
        {
            gameObject.SetActive(false);
        }
    }

    public bool CanInteract() => playerInside;

    public void Interact()
    {
        PickupTracker.Instance?.MarkPickedUp(itemID);
        InventoryManager.Instance.AddItem(itemData);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInside = false;
    }
}