using UnityEngine;

public class Lever2D : MonoBehaviour, IInteractable
{
    [Header("Coin")]
    public GameObject coinPrefab;
    public Transform spawnPoint;

    private bool playerInside;
    private bool alreadyUsed;

    public bool CanInteract()
    {
        if (!playerInside)
            return false;

        if (alreadyUsed)
            return false;

        return true;
    }

    public void Interact()
    {
        if (!CanInteract())
            return;

        if (InventoryManager.Instance.SelectedItem?.itemType != ItemType.Lever)
        {
            Debug.Log("Нужен выбранный рычаг");
            return;
        }

        Instantiate(coinPrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("Монета заспавнена");

        InventoryManager.Instance.ConsumeSelectedItem();
        alreadyUsed = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }
}
