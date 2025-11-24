using UnityEngine;

[CreateAssetMenu(fileName = "NewSeed", menuName = "Farming/Seed Data")]
public class SeedData : ScriptableObject
{
    [Header("Seed Info")]
    public string seedName;
    public Plant prefab;
    public Sprite icon;

    [Header("Harvest Info")]
    public string harvestName;
    public Sprite harvestIcon;
    public Plant harvestPrefab;
    public int harvestQty = 1;
}
