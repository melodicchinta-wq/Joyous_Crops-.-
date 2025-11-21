using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text qtyText;
    public GameObject highlight;
    public Button button;

    private int slotIndex;
    private InventoryUI ui;

    public void Init(int index, InventoryUI uiRef)
    {
        slotIndex = index;
        ui = uiRef;

        button.onClick.AddListener(() =>
        {
            ui.SelectSlot(slotIndex);
        });
    }

    public void SetSlot(SeedItem seed)
    {
        if (seed == null || seed.amount <= 0)
        {
            icon.enabled = false;
            qtyText.enabled = false;
        }
        else
        {
            icon.enabled = true;
            qtyText.enabled = true;
            icon.sprite = seed.icon;
            qtyText.text = seed.amount.ToString();
        }
    }

    public void Highlight(bool value)
    {
        highlight.SetActive(value);
    }
}
