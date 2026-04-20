using System.Collections.Generic;
using UnityEngine;

public class PickupTracker : MonoBehaviour
{
    public static PickupTracker Instance { get; private set; }

    // Список подобранных предметов
    // Ключ — уникальный ID предмета (сцена + имя объекта)
    private HashSet<string> pickedUpItems = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    // Запомнить что предмет подобран
    public void MarkPickedUp(string itemID)
    {
        pickedUpItems.Add(itemID);
    }

    // Проверить подобран ли предмет
    public bool IsPickedUp(string itemID)
    {
        return pickedUpItems.Contains(itemID);
    }
}