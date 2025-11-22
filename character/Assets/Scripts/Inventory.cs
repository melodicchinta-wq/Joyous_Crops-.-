using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SeedItem
{
    public string seedName;
    public Plant plantPrefab;
    public int amount;
    public Sprite icon;
}

public class Inventory : MonoBehaviour
{
    public List<SeedItem> seeds = new List<SeedItem>();
    public int activeSeedIndex = -1;

    public bool hasHoe = false;
    public bool hasWaterBottle = false;

    void Start()
    {
        seeds = new List<SeedItem>(); // mulai kosong
        activeSeedIndex = -1;
        hasHoe = false;
        hasWaterBottle = false;
    }

    // -------------------------------
    // SELECT ITEM
    // -------------------------------
    public void SelectIndex(int index)
    {
        if (index < 0 || index >= seeds.Count)
            activeSeedIndex = -1;
        else
            activeSeedIndex = index;
    }

    public SeedItem GetActiveSeedItem()
    {
        if (activeSeedIndex < 0 || activeSeedIndex >= seeds.Count)
            return null;
        return seeds[activeSeedIndex];
    }

    // -------------------------------
    // USE ITEM
    // -------------------------------
    public bool UseActiveSeed(int count = 1)
    {
        var s = GetActiveSeedItem();
        if (s == null || s.amount < count)
            return false;

        s.amount -= count;

        // jika habis → hapus slot
        if (s.amount <= 0)
        {
            seeds.RemoveAt(activeSeedIndex);
            activeSeedIndex = -1;
        }

        return true;
    }

    // -------------------------------
    // ADD ITEM
    // -------------------------------
    public void AddQuantity(string seedName, int qty, Plant prefab = null, Sprite icon = null)
    {
        // 1. Jika seed sudah ada → tambah jumlah
        for (int i = 0; i < seeds.Count; i++)
        {
            if (seeds[i].seedName == seedName)
            {
                seeds[i].amount += qty;
                return;
            }
        }

        // 2. Jika seed baru → tambahkan sebagai slot baru
        seeds.Add(new SeedItem
        {
            seedName = seedName,
            amount = qty,
            plantPrefab = prefab,
            icon = icon
        });
    }
}
