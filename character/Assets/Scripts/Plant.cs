using UnityEngine;

public class Plant : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite stage1;
    public Sprite stage2;
    public Sprite stage3;

    [Header("Growth")]
    public float growTimePerStage = 5f;
    public float waterDeadline = 7f;
    public float fertilizerSpeedMultiplier = 1.7f;

    [Header("Harvest Info")]
    public int maxStage = 2;  // stage 0,1,2 → 2 = matang total

    [Header("Seed Data")]
    public SeedData seedData; // dari ScriptableObject

    private SpriteRenderer sr;
    private int currentStage = 0;
    private float stageTimer = 0f;
    private float timeSincePlanted = 0f;
    private bool watered = false;
    private bool fertilized = false;
    private bool dead = false;

    private SoilTile soil;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    public void SetSoil(SoilTile s) => soil = s;
    public bool IsDead() => dead;

    public bool IsReadyToHarvest()
    {
        return !dead && currentStage >= maxStage;
    }

    public void OnWatered() => watered = true;
    public void OnFertilized() => fertilized = true;

    void Update()
    {
        if (dead) return;

        timeSincePlanted += Time.deltaTime;

        // tanaman mati kalau tidak disiram
        if (!watered && timeSincePlanted >= waterDeadline)
        {
            Die();
            return;
        }

        // kalau sudah disiram → mulai tumbuh
        if (watered)
        {
            float speed = fertilized ? fertilizerSpeedMultiplier : 1f;
            stageTimer += Time.deltaTime * speed;

            if (currentStage == 0 && stageTimer >= growTimePerStage)
            {
                currentStage = 1;
                stageTimer = 0;
                UpdateSprite();
            }
            else if (currentStage == 1 && stageTimer >= growTimePerStage)
            {
                currentStage = 2;  // stage matang
                stageTimer = 0;
                UpdateSprite();
                soil?.OnPlantReady(); // callback tanah
            }
        }
    }

    void UpdateSprite()
    {
        if (sr == null) return;

        if (currentStage == 0) sr.sprite = stage1;
        else if (currentStage == 1) sr.sprite = stage2;
        else sr.sprite = stage3; // stage3 = tahap matang visual
    }

    public void Die()
    {
        dead = true;

        if (sr != null)
            sr.color = new Color(0.4f, 0.3f, 0.3f); // warna mati
    }

    // dipanggil ketika baru ditanam
    public void ResetPlant()
    {
        currentStage = 0;
        stageTimer = 0;
        timeSincePlanted = 0;
        watered = false;
        fertilized = false;
        dead = false;

        if (sr != null)
            sr.color = Color.white;

        UpdateSprite();
    }
}
