using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public InventorySlotUI[] slots;

    private int selectedIndex = -1;

    void Start() => UpdateUI();
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
    }

    public void SelectSlot(int index)
    {
        selectedIndex = index;
        inventory.activeSeedIndex = index;
        UpdateHighlight();
    }
    void UpdateHighlight()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            var seed = slots[i].GetCurrentSeed();
            bool isActive = (seed != null && selectedIndex >= 0 &&
                             selectedIndex < inventory.seeds.Count &&
                             seed == inventory.seeds[selectedIndex]);
            slots[i].SetHighlight(isActive);
        }
    }

    public void UpdateUI()
    {
        var tempSeeds = new List<SeedItem>(inventory.seeds);

        for (int i = 0; i < slots.Length; i++)
        {
            SeedItem seedToShow = tempSeeds.Find(s => s.amount > 0);
            if (seedToShow != null)
            {
                slots[i].SetSlot(seedToShow);
                tempSeeds.Remove(seedToShow);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }

        UpdateHighlight();
    }

}
