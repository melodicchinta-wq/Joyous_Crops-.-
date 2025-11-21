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
    public int maxHarvestBonus = 2; // maksimal bonus saat panen
}
public class Inventory : MonoBehaviour
{
    public List<SeedItem> seeds = new List<SeedItem>();
    public int activeSeedIndex = -1;

    public event Action OnInventoryChanged;

    public void SelectIndex(int index)
    {
        if (index < 0 || index >= seeds.Count) activeSeedIndex = -1;
        else activeSeedIndex = index;
    }

    public SeedItem GetActiveSeedItem()
    {
        if (activeSeedIndex < 0 || activeSeedIndex >= seeds.Count) return null;
        return seeds[activeSeedIndex];
    }

    public bool UseActiveSeed()
    {
        var s = GetActiveSeedItem();
        if (s == null || s.amount <= 0) return false;
        s.amount--;
        if (s.amount <= 0) activeSeedIndex = -1;

        OnInventoryChanged?.Invoke();
        return true;
    }

    public void AddQuantity(string seedName, int qty)
    {
        foreach (var s in seeds)
        {
            if (s.seedName == seedName)
            {
                s.amount += qty;
                OnInventoryChanged?.Invoke();
                return;
            }
        }
    }
}
