using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public InventorySlotUI[] slots;

    private int selectedIndex = 0;

    void Start()
    {
        // Init semua slot
        for (int i = 0; i < slots.Length; i++)
            slots[i].Init(i, this);

        // Subscribe event inventory
        if (inventory != null)
            inventory.OnInventoryChanged += UpdateUI;

        UpdateUI();
        HighlightSelectedSlot(selectedIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
    }

    public void SelectSlot(int index)
    {
        selectedIndex = index;
        inventory.SelectIndex(index);
        HighlightSelectedSlot(index);
    }

    void HighlightSelectedSlot(int index)
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].Highlight(i == index);
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.seeds.Count)
                slots[i].SetSlot(inventory.seeds[i]);
            else
                slots[i].SetSlot(new SeedItem { amount = 0 }); // slot kosong
        }
    }

    private void OnDestroy()
    {
        if (inventory != null)
            inventory.OnInventoryChanged -= UpdateUI;
    }
}
