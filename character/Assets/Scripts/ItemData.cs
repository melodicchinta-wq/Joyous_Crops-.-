using UnityEngine;

public enum ItemType
{
    Seed,
    Product,
    Tool
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Farming/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public ItemType type;
    public Sprite icon;
    public Plant plantPrefab;   // only for seed (optional)
}