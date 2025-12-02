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
    public int maxCount = 8;  // NEW!! max per slot
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
    public SeedItem GetSeed(string name)
    {
        return seeds.Find(s => s.seedName == name);
    }
    // panggil ini ketika slot UI sudah dipakai untuk mengurangi inventory "source of truth"
    public bool DecreaseQuantityByName(string seedName, int qty = 1)
    {
        var item = seeds.Find(s => s.seedName == seedName);
        if (item == null) return false;

        item.amount -= qty;
        if (item.amount <= 0)
        {
            seeds.Remove(item);
        }
        return true;
    }
    public List<SeedItem> AddItemWithMax(string seedName, int qty, Plant prefab = null, Sprite icon = null)
    {
        List<SeedItem> newStacks = new List<SeedItem>();
        int remaining = qty;

        // 1. Coba isi slot-slot yang sudah ada
        foreach (var item in seeds)
        {
            if (item.seedName == seedName && item.amount < item.maxCount)
            {
                int canAdd = item.maxCount - item.amount;
                int add = Mathf.Min(canAdd, remaining);

                item.amount += add;
                remaining -= add;

                if (remaining <= 0)
                    return newStacks;
            }
        }

        // 2. Kalau masih ada sisa → buat slot baru
        while (remaining > 0)
        {
            int add = Mathf.Min(remaining, 8);

            SeedItem newItem = new SeedItem
            {
                seedName = seedName,
                plantPrefab = prefab,
                icon = icon,
                amount = add,
                maxCount = 8
            };

            seeds.Add(newItem);
            newStacks.Add(newItem);

            remaining -= add;
        }

        return newStacks; // ini nanti dipakai InventoryUI supaya tampil slot-slot baru
    }
    public bool TryMerge(SeedItem a, SeedItem b)
    {
        if (a == null || b == null) return false;
        if (a.seedName != b.seedName) return false; // beda jenis ga bisa merge

        int maxStack = a.maxCount;

        int space = maxStack - a.amount;
        if (space <= 0) return false; // sudah full

        int move = Mathf.Min(space, b.amount);

        a.amount += move;
        b.amount -= move;

        return true;
    }

}
