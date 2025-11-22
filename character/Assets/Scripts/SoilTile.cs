using UnityEngine;

public class SoilTile : MonoBehaviour
{
    public enum SoilState { Grass, Hoed, Watered }
    public SoilState currentState = SoilState.Grass;

    [Header("Sprites")]
    public Sprite grassSprite;
    public Sprite hoedSprite;
    public Sprite wateredSprite;

    [Header("Spawn")]
    public Transform plantSpawnPoint;
    [HideInInspector] public Plant currentPlant;

    [Header("References")]
    public Inventory playerInventory;   // ← PENTING

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    // HOE
    public void Hoe()
    {
        if (currentState != SoilState.Grass) return;
        currentState = SoilState.Hoed;
        UpdateSprite();
    }

    // PLANT SEED
    public void PlantSeed(Plant plantPrefab)
    {
        if (plantPrefab == null) return;
        if (currentState != SoilState.Hoed) return;

        currentPlant = Instantiate(plantPrefab, plantSpawnPoint.position, Quaternion.identity);
        currentPlant.SetSoil(this);

        currentPlant.ResetPlant();
        currentState = SoilState.Hoed;
        UpdateSprite();
    }

    // WATER
    public void Water()
    {
        if (currentPlant != null)
            currentPlant.OnWatered();

        currentState = SoilState.Watered;
        UpdateSprite();
    }

    // FERTILIZE
    public void Fertilize()
    {
        if (currentPlant == null) return;
        currentPlant.OnFertilized();
    }

    // HARVEST
    public (string name, int qty, Plant prefab, Sprite icon) Harvest()
    {
        if (currentPlant == null)
            return (null, 0, null, null);

        if (currentPlant.seedData == null)
        {
            Debug.LogError("Plant tidak punya SeedData! Assign seedData di prefab Plant!");
            return (null, 0, null, null);
        }

        string seedName = currentPlant.seedData.seedName;
        Plant prefabAsli = currentPlant.seedData.prefab;
        Sprite iconAsli = currentPlant.seedData.icon;

        int qty = Random.Range(1, 3);

        Destroy(currentPlant.gameObject);
        currentPlant = null;

        currentState = SoilState.Hoed;
        UpdateSprite();

        return (seedName, qty, prefabAsli, iconAsli);
    }

    // REMOVE DEAD PLANT
    public void RemoveDeadPlant()
    {
        if (currentPlant == null) return;

        if (currentPlant.IsDead())
        {
            Destroy(currentPlant.gameObject);
            currentPlant = null;
            currentState = SoilState.Hoed;
            UpdateSprite();
        }
    }

    // RESET SOIL
    public void ResetSoil()
    {
        if (currentPlant != null)
            Destroy(currentPlant.gameObject);

        currentPlant = null;
        currentState = SoilState.Grass;
        UpdateSprite();
    }

    // CALLBACK READY
    public void OnPlantReady()
    {
        Debug.Log("Tanaman siap panen.");
    }

    // UPDATE SPRITE
    void UpdateSprite()
    {
        sr.sprite = currentState switch
        {
            SoilState.Grass => grassSprite,
            SoilState.Hoed => hoedSprite,
            SoilState.Watered => wateredSprite,
            _ => grassSprite
        };
    }
}
