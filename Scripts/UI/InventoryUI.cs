using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [SerializeField] private Transform itemsContainer;
    [SerializeField] private UIItemSlot slotPrefab;

    private Dictionary<ItemData, UIItemSlot> slots = new();

    private void Awake()
    {
        Instance = this;
    }

    public void AddItem(ItemData item)
    {
        var slot = Instantiate(slotPrefab, itemsContainer);
        slot.Setup(item);
        slots[item] = slot;
    }

    public void RemoveItem(ItemData item)
    {
        if (!slots.ContainsKey(item)) return;

        Destroy(slots[item].gameObject);
        slots.Remove(item);
    }

    public void Highlight(ItemData item)
{
    Debug.Log("Highlight вызван");

    foreach (var s in slots.Values)
        s.SetSelected(false);

    if (slots.ContainsKey(item))
    {
        Debug.Log("Слот найден, включаем рамку");
        slots[item].SetSelected(true);
    }
    else
    {
        Debug.LogError("Слот НЕ найден в словаре");
    }
}
}
