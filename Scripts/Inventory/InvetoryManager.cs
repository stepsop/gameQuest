using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private List<ItemData> items = new();
    public ItemData SelectedItem { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(ItemData item)
    {
        items.Add(item);
        InventoryUI.Instance.AddItem(item);
    }

    public bool HasItem(ItemType type)
    {
        return items.Exists(i => i.itemType == type);
    }

  public void SelectItem(ItemData item)
{
    Debug.Log("SelectItem вызван: " + item.itemType);
    SelectedItem = item;

    if (InventoryUI.Instance == null)
    {
        Debug.LogError("InventoryUI.Instance = NULL");
        return;
    }

    InventoryUI.Instance.Highlight(item);
}

    public void ConsumeSelectedItem()
    {
        if (SelectedItem == null) return;

        if (SelectedItem.consumable)
        {
            items.Remove(SelectedItem);
            InventoryUI.Instance.RemoveItem(SelectedItem);
            SelectedItem = null;
        }
    }
}
