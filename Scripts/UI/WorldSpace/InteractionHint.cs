using UnityEngine;
using TMPro;

public class InteractionHint : MonoBehaviour
{
    [Header("Префаб подсказки")]
    [SerializeField] private GameObject hintPrefab;

    [Header("Настройки")]
    [SerializeField] private float heightOffset = 1f;

    private GameObject hintObject;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        // Создаём префаб НЕ как дочерний — а в корень сцены
        // Это важно! Так масштаб родителя не влияет на размер подсказки
        hintObject = Instantiate(hintPrefab);

        TMP_Text label = hintObject.GetComponentInChildren<TMP_Text>();
        if (label != null)
        {
            // Пробуем взять название из ItemData SO
            // Если нет PickupItem — падаем на имя GameObject
            PickupItem pickup = GetComponent<PickupItem>();
            if (pickup != null && pickup.itemData != null)
                label.text = pickup.gameObject.name;
            else
                label.text = gameObject.name;
        }

        hintObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (hintObject == null || !hintObject.activeSelf) return;

        hintObject.transform.position = transform.position + Vector3.up * heightOffset;
        hintObject.transform.rotation = mainCamera.transform.rotation;
    }

    public void Show() => hintObject?.SetActive(true);
    public void Hide() => hintObject?.SetActive(false);

    private void OnDestroy()
    {
        if (hintObject != null)
            Destroy(hintObject);
    }
}