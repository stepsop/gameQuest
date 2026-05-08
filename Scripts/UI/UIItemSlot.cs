using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIItemSlot : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private Image selectionFrame;
    [SerializeField] private TextMeshProUGUI amountText;

    [Header("Tooltip")]
    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TextMeshProUGUI tooltipText;

    private ItemData item;
    private int currentAmount;

    // Статические — одни на весь инвентарь во время drag
    private static GameObject dragIcon;
    private static UIItemSlot dragSource;

    public ItemData Item => item;

    public void Setup(ItemData data, int amount)
    {
        item = data;
        currentAmount = amount;
        icon.sprite = data.icon;
        amountText.text = amount > 1 ? amount.ToString() : "";
        selectionFrame.enabled = false;

        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }

    // --- TOOLTIP ---

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null || tooltipPanel == null) return;
        tooltipText.text = item.itemName;
        tooltipPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }

    // --- DRAG & DROP ---

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;

        dragSource = this;

        // Скрываем tooltip во время drag
        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);

        // Создаём иконку которая летит за курсором
        dragIcon = new GameObject("DragIcon");
        dragIcon.transform.SetParent(GetRootCanvas().transform, false);
        dragIcon.transform.SetAsLastSibling();

        var img = dragIcon.AddComponent<Image>();
        img.sprite = icon.sprite;
        // raycastTarget = false — иначе иконка перехватит событие и IDropHandler не сработает
        img.raycastTarget = false;

        dragIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);

        // Делаем слот полупрозрачным пока тащим
        icon.color = new Color(1, 1, 1, 0.4f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
            dragIcon.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        icon.color = Color.white;

        // Уничтожаем иконку всегда — даже если дроп сработал
        if (dragIcon != null)
        {
            Destroy(dragIcon);
            dragIcon = null;
        }

        dragSource = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (dragSource == null || dragSource == this) return;
        if (dragSource.item == item) return;

        // Чистим визуал до того как инвентарь пересоздаст слоты
        dragSource.ResetDragVisual();

        CombineManager.Instance.TryCombine(dragSource.item, item);
    }

    public void OnClick()
    {
        InventoryManager.Instance.SelectItem(item);
    }

    public void SetSelected(bool value)
    {
        selectionFrame.enabled = value;
    }

    private Canvas GetRootCanvas()
    {
        var canvas = GetComponentInParent<Canvas>();
        while (canvas.transform.parent != null &&
               canvas.transform.parent.GetComponentInParent<Canvas>() != null)
            canvas = canvas.transform.parent.GetComponentInParent<Canvas>();
        return canvas;
    }
    public void ResetDragVisual()
    {
        icon.color = Color.white;
        if (dragIcon != null)
        {
            Destroy(dragIcon);
            dragIcon = null;
        }
    }
}