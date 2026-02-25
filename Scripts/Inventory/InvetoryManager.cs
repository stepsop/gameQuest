using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private List<ItemData> items = new();
    public List<ItemData> Items => items;

    public ItemData SelectedItem { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(ItemData item)
    {
        items.Add(item);
        InventoryUI.Instance.RefreshUI(items);
    }

    public bool HasItem(ItemType type)
    {
        return items.Exists(i => i.itemType == type);
    }

    public void SelectItem(ItemData item)
    {
        if (SelectedItem == item)
        {
            SelectedItem = null;
            InventoryUI.Instance.ClearSelection();
            return;
        }

        SelectedItem = item;
        InventoryUI.Instance.Highlight(item);
    }

    public void ConsumeSelectedItem()
    {
        if (SelectedItem == null) return;

        if (SelectedItem.consumable)
        {
            items.Remove(SelectedItem);
            SelectedItem = null;

            InventoryUI.Instance.RefreshUI(items);
        }
    }
}