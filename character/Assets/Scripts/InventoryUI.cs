using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public InventorySlotUI[] slots;

    [HideInInspector]
    public int selectedIndex = 0;

    void Start()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].Init(i, this);

        LoadInventory();
        UpdateHighlight();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
    }

    public void SelectSlot(int slotIndex)
    {
        selectedIndex = slotIndex;

        // Ambil item dari slot UI berdasarkan index
        SeedItem seedInSlot = slots[slotIndex].GetCurrentSeed();

        // update activeSeedIndex sesuai posisi seed di inventory
        if (seedInSlot != null)
        {
            inventory.activeSeedIndex = inventory.seeds.IndexOf(seedInSlot);
        }
        else
        {
            inventory.activeSeedIndex = -1; // slot kosong
        }

        UpdateHighlight();
    }


    void UpdateHighlight()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            bool isActive = (i == selectedIndex);
            slots[i].SetHighlight(isActive);
        }
    }


    public void UpdateUI()
    {
        // hanya update qty dan icon
        foreach (var slot in slots)
        {
            var seed = slot.GetCurrentSeed();
            if (seed != null && seed.amount > 0)
            {
                slot.icon.sprite = seed.icon;
                slot.icon.gameObject.SetActive(true);
                slot.qtyText.text = seed.amount.ToString();
            }
            else
            {
                slot.ClearSlot();
            }
        }

        UpdateHighlight();
    }

    public void SwapSlots(int a, int b)
    {
        if (a < 0 || b < 0) return;
        if (a >= inventory.seeds.Count || b >= inventory.seeds.Count) return;

        // swap data di Inventory
        var temp = inventory.seeds[a];
        inventory.seeds[a] = inventory.seeds[b];
        inventory.seeds[b] = temp;

        // perbaiki activeSeedIndex jika terkena swap
        if (inventory.activeSeedIndex == a) inventory.activeSeedIndex = b;
        else if (inventory.activeSeedIndex == b) inventory.activeSeedIndex = a;

        UpdateUI();
    }
    public void SyncInventoryWithUI()
    {
        if (inventory == null) return;

        inventory.seeds.Clear();
        foreach (var slot in slots)
        {
            var seed = slot.GetCurrentSeed();
            if (seed != null)
                inventory.seeds.Add(seed);
        }
    }

    public void LoadInventory()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.seeds.Count && inventory.seeds[i].amount > 0)
                slots[i].SetSlot(inventory.seeds[i]);
            else
                slots[i].ClearSlot();
        }
    }

    public void AddItemToEmptySlot(SeedItem item)
    {
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                slot.SetSlot(item);
                return;
            }
        }

        Debug.Log("Tidak ada slot kosong!");
    }
    // cek apakah slot sudah berisi seed dengan nama ini
    private int FindSlotIndexBySeedName(string seedName)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            var s = slots[i].GetCurrentSeed();
            if (s != null && s.seedName == seedName) return i;
        }
        return -1;
    }

    // dipanggil setelah inventory berubah (mis. panen). 
    // Niat: masukkan item baru dari inventory ke slot (stack jika ada, atau slot kosong).
    public void ApplyInventoryAddsToSlots()
    {
        if (inventory == null) return;

        // untuk setiap item di inventory, coba stack ke slot yg sudah ada,
        // atau isi slot kosong jika belum ada.
        foreach (var invItem in inventory.seeds.ToArray()) // toArray agar aman jika kita modify inventory
        {
            if (invItem == null) continue;

            // 1) cari slot yg sudah punya item sama -> stack
            int existingSlot = FindSlotIndexBySeedName(invItem.seedName);
            if (existingSlot >= 0)
            {
                var slot = slots[existingSlot];
                // jika slot sudah menampung reference item yg berasal dari inventory, cukup update qty
                // tapi untuk aman: perbarui slot supaya icon/amount sinkron
                slot.SetSlot(invItem);
                continue;
            }

            // 2) cari slot kosong
            int emptyIndex = -1;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].IsEmpty) { emptyIndex = i; break; }
            }

            if (emptyIndex >= 0)
            {
                slots[emptyIndex].SetSlot(invItem);
                continue;
            }

            // 3) tidak ada slot kosong, biarkan tetap di inventory (tidak hilang)
        }

        // akhir: refresh tampilan qty/icon/highlight
        UpdateUI();
    }

}