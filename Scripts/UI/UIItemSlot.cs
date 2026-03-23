using UnityEngine;
using UnityEngine.UI;
using TMPro; // если используете TextMeshPro

public class UIItemSlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image selectionFrame;
    [SerializeField] private TextMeshProUGUI amountText; // 📦 Текст для количества

    private ItemData item;
    private int currentAmount;

    public void Setup(ItemData data, int amount)
    {
        item = data;
        currentAmount = amount;
        icon.sprite = data.icon;
        
        // Показываем количество, только если его больше 1
        if (amount > 1)
            amountText.text = amount.ToString();
        else
            amountText.text = "";
            
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