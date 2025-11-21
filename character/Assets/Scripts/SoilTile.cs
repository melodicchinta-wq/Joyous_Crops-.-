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
    [HideInInspector] public Plant currentPlant; // visible di inspector (untuk debug)

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    public void Hoe()
    {
        if (currentState != SoilState.Grass) return;
        currentState = SoilState.Hoed;
        UpdateSprite();
    }

    // plantPrefab datang dari player
    public void PlantSeed(Plant plantPrefab)
    {
        if (plantPrefab == null) return;
        if (currentState != SoilState.Hoed) return;

        currentPlant = Instantiate(plantPrefab, plantSpawnPoint.position, Quaternion.identity);
        currentPlant.SetSoil(this);
        currentState = SoilState.Hoed; // tetap coklat saat ada tanaman
        UpdateSprite();
    }

    // memanggil watering pada plant (jika ada) — juga ubah sprite tanah jadi watered
    public void Water()
    {
        if (currentState != SoilState.Hoed && currentState != SoilState.Watered) return;
        if (currentPlant != null)
        {
            currentPlant.OnWatered();
        }

        currentState = SoilState.Watered;
        UpdateSprite();
    }

    public void Fertilize()
    {
        if (currentPlant == null) return;
        currentPlant.OnFertilized();
        // optionally change sprite slightly (we keep same watered sprite)
    }

    // dipanggil saat panen; jika tanaman siap dan tidak mati -> return result
    public (string name, int qty) Harvest()
    {
        if (currentPlant == null) return (null, 0);
        if (currentPlant.IsDead()) return (null, 0);
        if (!currentPlant.IsReady()) return (null, 0);

        var res = currentPlant.Harvest();
        Destroy(currentPlant.gameObject);
        currentPlant = null;

        // setelah panen, tanah tetap hoed (coklat)
        currentState = SoilState.Hoed;
        UpdateSprite();

        return res;
    }

    // buang tanaman mati
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

    public void ResetSoil()
    {
        if (currentPlant != null) Destroy(currentPlant.gameObject);
        currentPlant = null;
        currentState = SoilState.Grass;
        UpdateSprite();
    }

    public void OnPlantReady()
    {
        // callback jika mau (kosong untuk sekarang)
    }

    void UpdateSprite()
    {
        switch (currentState)
        {
            case SoilState.Grass: sr.sprite = grassSprite; break;
            case SoilState.Hoed: sr.sprite = hoedSprite; break;
            case SoilState.Watered: sr.sprite = wateredSprite; break;
        }
    }
}
