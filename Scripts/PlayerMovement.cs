using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private PlayerInputActions input;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = new PlayerInputActions();
        input.Enable();
    }

    private void FixedUpdate()
    {
        // 🔴 БЛОК ДВИЖЕНИЯ во время диалога
        if (GameState.IsDialogueOpen)
        {
            rb.linearVelocity = Vector2.zero; // гарантированно останавливаем игрока
            return;
        }

        // 🟢 Движение
        Vector2 move = input.Player.Move.ReadValue<Vector2>().normalized;
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        // 🔴 БЛОК ВСЕГО ВВОДА во время диалога
        if (GameState.IsDialogueOpen)
            return;

        // 🟢 Кнопка взаимодействия (E)
        if (input.Player.Interact.WasPressedThisFrame())
        {
            Debug.Log("STEP 3: Нажата E");
            TryInteract();
        }
    }

    void TryInteract()
    {
        // 🔍 Ищем объекты рядом с игроком
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.2f);

        foreach (var hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();

            // Если нашли объект и с ним можно взаимодействовать
            if (interactable != null && interactable.CanInteract())
            {
                Debug.Log("Player: взаимодействую с объектом");
                interactable.Interact();
                return;
            }
        }

        Debug.Log("Player: рядом нет объектов для взаимодействия");
    }

    private void OnDestroy()
    {
        input.Disable();
    }
}