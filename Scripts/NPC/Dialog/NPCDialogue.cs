using UnityEngine;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueData startDialogue;

    private bool playerInside;

    public bool CanInteract()
    {
        return playerInside;
    }

    public void Interact()
    {
        if (!CanInteract()) return;

        DialogueUI.Instance.OpenDialogue(startDialogue);
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