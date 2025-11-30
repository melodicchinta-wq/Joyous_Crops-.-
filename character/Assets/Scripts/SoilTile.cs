using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
        var list = new List<(string, int, Plant, Sprite)>();

        if (currentPlant == null)
            return list;

        // CEK SIAP PANEN
        if (!currentPlant.IsReadyToHarvest())
        {
            Debug.Log("Belum siap panen!");
            return list;
        }

        var seed = currentPlant.seedData;

        // RANDOM JUMLAH
        int seedQtyRandom = Random.Range(1, 3);     // min 1 max 2
        int harvestQtyRandom = Random.Range(1, 3);  // min 1 max 2

        // 1. RANDOM SEED
        list.Add((seed.seedName, seedQtyRandom, seed.prefab, seed.icon));

        // 2. RANDOM WORTEL UTUH
        list.Add((seed.harvestName, harvestQtyRandom, seed.harvestPrefab, seed.harvestIcon));

        Destroy(currentPlant.gameObject);
        currentPlant = null;

        currentState = SoilState.Hoed;
        UpdateSprite();

        return list;
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
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Tidak bisa reset tanah saat editor tidak sedang Play.");
            return;
        }

        if (currentPlant != null)
        {
            if (currentPlant.gameObject.scene.IsValid())   // hanya destroy object yg ada di scene
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