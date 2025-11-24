using System.Collections.Generic;
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
        if (currentPlant != null)
            currentPlant.OnFertilized();
    }

    // HARVEST
    public List<(string name, int qty, Plant prefab, Sprite icon)> Harvest()
    {
        var result = new List<(string, int, Plant, Sprite)>();

        if (currentPlant == null)
            return result;

        // CEK SIAP PANEN
        if (!currentPlant.IsReadyToHarvest())
        {
            Debug.Log("Belum siap panen!");
            return result;
        }

        // Akses seedData
        SeedData sd = currentPlant.seedData;

        if (sd == null)
        {
            Debug.LogError("SeedData belum di-assign pada Plant!");
            return result;
        }

        // RANDOM JUMLAH
        int seedQty = Random.Range(1, 3);    // min 1 max 2
        int harvestQty = Random.Range(1, 3); // min 1 max 2

        // Tambah seed (hasil panen)
        result.Add((sd.seedName, seedQty, sd.prefab, sd.icon));

        // Tambah tanaman utuh (wortel)
        result.Add((sd.harvestName, harvestQty, sd.harvestPrefab, sd.harvestIcon));

        // bersihkan
        Destroy(currentPlant.gameObject);
        currentPlant = null;

        currentState = SoilState.Hoed;
        UpdateSprite();

        return result;
    }

    // REMOVE DEAD PLANT
    public void RemoveDeadPlant()
    {
        if (currentPlant == null) return;

        if (currentPlant.IsDead())   // ← SEKARANG TIDAK MERAH
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
        {
            Destroy(currentPlant.gameObject);
        }

        currentPlant = null;
        currentState = SoilState.Grass;
        UpdateSprite();
    }

    // CALLBACK READY
    public void OnPlantReady()
    {
        Debug.Log("Tanaman siap panen.");
    }

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
