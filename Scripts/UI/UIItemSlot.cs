using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image selectionFrame;

    private ItemData item;

    public void Setup(ItemData data)
    {
        item = data;
        icon.sprite = data.icon;
        selectionFrame.enabled = false;
    }

    public void OnClick()
    {
        Debug.Log("Слот нажат");
        InventoryManager.Instance.SelectItem(item);
    }

    public void SetSelected(bool value)
{
    Debug.Log("SetSelected: " + value);
    selectionFrame.enabled = value;
}
    
}
