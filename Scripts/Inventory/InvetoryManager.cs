using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    // Теперь это будет список, где мы храним не просто предметы, а предметы с количеством
    private List<ItemStack> items = new();
    public List<ItemStack> Items => items;

    public ItemData SelectedItem { get; private set; }

    [System.Serializable]
    public class ItemStack
    {
        public ItemData itemData;
        public int amount;

        public ItemStack(ItemData data, int qty)
        {
            itemData = data;
            amount = qty;
        }
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 📦 НОВЫЙ МЕТОД: Добавление с количеством
    public void AddItem(ItemData item, int amount = 1)
    {
        // Ищем, есть ли уже такой предмет в инвентаре
        ItemStack existingStack = items.Find(stack => stack.itemData == item);

        if (existingStack != null)
        {
            // Если нашли — увеличиваем количество
            existingStack.amount += amount;
        }
        else
        {
            // Если нет — создаем новую запись
            items.Add(new ItemStack(item, amount));
        }

        // Обновляем UI (нужно будет чуть изменить InventoryUI)
        InventoryUI.Instance.RefreshUI(items);
    }

    // Старый метод AddItem (для обратной совместимости, если где-то использовался)
    public void AddItem(ItemData item)
    {
        AddItem(item, 1);
    }

    public bool HasItem(ItemType type)
    {
        return items.Exists(stack => stack.itemData.itemType == type);
    }

    // Получить количество предметов определенного типа
    public int GetItemCount(ItemType type)
    {
        ItemStack stack = items.Find(s => s.itemData.itemType == type);
        return stack?.amount ?? 0;
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

    // 💰 Трата монет (например, при покупке)
    public bool SpendCoins(int amount)
    {
        ItemStack coinStack = items.Find(s => s.itemData.itemType == ItemType.Coin);
        
        if (coinStack == null || coinStack.amount < amount)
            return false; // Недостаточно монет

        coinStack.amount -= amount;
        
        // Если монет стало 0 — удаляем запись
        if (coinStack.amount <= 0)
            items.Remove(coinStack);

        InventoryUI.Instance.RefreshUI(items);
        return true;
    }

    // Потребление выбранного предмета (теперь с учетом количества)
    public void ConsumeSelectedItem()
    {
        if (SelectedItem == null) return;

        if (SelectedItem.consumable)
        {
            ItemStack stack = items.Find(s => s.itemData == SelectedItem);
            
            if (stack != null)
            {
                stack.amount--;
                
                if (stack.amount <= 0)
                {
                    items.Remove(stack);
                    SelectedItem = null;
                }
            }

            InventoryUI.Instance.RefreshUI(items);
        }
    }
}