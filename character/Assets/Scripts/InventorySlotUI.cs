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
    private SeedItem currentSeed;

    public void Init(int index, InventoryUI uiRef)
    {
        slotIndex = index;
        ui = uiRef;
        button.onClick.AddListener(() => ui.SelectSlot(slotIndex));
    }

    public void SetSlot(SeedItem seed)
    {
        currentSeed = seed;
        if (seed != null && seed.amount > 0)
        {
            icon.gameObject.SetActive(true);
            icon.sprite = seed.icon;
            qtyText.text = seed.amount.ToString();
        }
        else
        {
            ClearSlot();
        }
    }


    public void ClearSlot()
    {
        currentSeed = null;
        icon.gameObject.SetActive(false);
        qtyText.text = "";
        highlight.SetActive(false);
    }
    public void SetHighlight(bool value)
    {
        if (highlight != null)
            highlight.SetActive(value);
    }

    public SeedItem GetCurrentSeed()
    {
        return currentSeed;
    }
}