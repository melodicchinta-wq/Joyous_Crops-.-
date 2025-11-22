using UnityEngine;

[CreateAssetMenu(fileName = "NewSeed", menuName = "Farming/Seed Data")]
public class SeedData : ScriptableObject
{
    public string seedName;
    public Plant prefab;
    public Sprite icon;
}
