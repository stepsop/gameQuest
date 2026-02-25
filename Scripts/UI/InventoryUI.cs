using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    // Singleton — чтобы можно было обращаться из InventoryManager
    public static InventoryUI Instance { get; private set; }

    [Header("UI References")]

    // Контейнер, в который будут создаваться слоты
    [SerializeField] private Transform itemsContainer;

    // Префаб одного слота (должен быть prefab из Project!)
    [SerializeField] private UIItemSlot slotPrefab;

    // Сколько предметов показывать на одной странице
    [SerializeField] private int itemsPerPage = 6;

    // Кнопки перелистывания
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject prevButton;

    // Текущая страница (начинаем с 0)
    private int currentPage = 0;

    // Словарь слотов ТЕКУЩЕЙ страницы
    private Dictionary<ItemData, UIItemSlot> slots = new();

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Главный метод перерисовки UI.
    /// Вызывается каждый раз, когда меняется список предметов.
    /// </summary>
    public void RefreshUI(List<ItemData> items)
    {
        // 1️⃣ Удаляем старые слоты
        foreach (Transform child in itemsContainer)
            Destroy(child.gameObject);

        slots.Clear();

        // 2️⃣ Считаем сколько всего страниц
        int totalPages = Mathf.CeilToInt((float)items.Count / itemsPerPage);

        // Если предметов 0 — всё равно считаем 1 страницу,
        // чтобы не было деления на ноль
        totalPages = Mathf.Max(totalPages, 1);

        // 3️⃣ Защита: если текущая страница стала больше допустимой
        if (currentPage >= totalPages)
            currentPage = totalPages - 1;

        // 4️⃣ Вычисляем диапазон предметов для текущей страницы
        int startIndex = currentPage * itemsPerPage;
        int endIndex = Mathf.Min(startIndex + itemsPerPage, items.Count);

        // 5️⃣ Создаём слоты только для нужных предметов
        for (int i = startIndex; i < endIndex; i++)
        {
            ItemData item = items[i];

            UIItemSlot slot = Instantiate(slotPrefab, itemsContainer);
            slot.Setup(item);

            slots[item] = slot;
        }

        // 6️⃣ Обновляем кнопки перелистывания
        UpdatePageButtons(totalPages);
    }

    /// <summary>
    /// Обновляет видимость кнопок Next/Prev
    /// </summary>
    private void UpdatePageButtons(int totalPages)
    {
        // Кнопка "вперёд" видна,
        // если текущая страница не последняя
        nextButton.SetActive(currentPage < totalPages - 1);

        // Кнопка "назад" видна,
        // если мы не на первой странице
        prevButton.SetActive(currentPage > 0);
    }

    /// <summary>
    /// Выделение выбранного предмета
    /// </summary>
    public void Highlight(ItemData item)
    {
        // Сначала снимаем выделение со всех слотов текущей страницы
        foreach (var s in slots.Values)
            s.SetSelected(false);

        // Если предмет есть на текущей странице — выделяем
        if (slots.ContainsKey(item))
            slots[item].SetSelected(true);
    }

    /// <summary>
    /// Снять выделение со всех слотов
    /// </summary>
    public void ClearSelection()
    {
        foreach (var s in slots.Values)
            s.SetSelected(false);
    }

    /// <summary>
    /// Перейти на следующую страницу
    /// </summary>
    public void NextPage()
    {
        currentPage++;
        RefreshUI(InventoryManager.Instance.Items);
    }

    /// <summary>
    /// Перейти на предыдущую страницу
    /// </summary>
    public void PrevPage()
    {
        currentPage--;
        RefreshUI(InventoryManager.Instance.Items);
    }
}