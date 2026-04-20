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

        // Ставим название объекта как текст
        TMP_Text label = hintObject.GetComponentInChildren<TMP_Text>();
        if (label != null)
            label.text = gameObject.name;

        hintObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (hintObject == null || !hintObject.activeSelf) return;

        // Каждый кадр двигаем подсказку над объектом вручную
        hintObject.transform.position = transform.position + Vector3.up * heightOffset;

        // Поворачиваем к камере
        hintObject.transform.rotation = mainCamera.transform.rotation;
    }

    public void Show() => hintObject?.SetActive(true);
    public void Hide() => hintObject?.SetActive(false);

    // Когда объект уничтожается — удаляем подсказку
    private void OnDestroy()
    {
        if (hintObject != null)
            Destroy(hintObject);
    }
}