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
    public string seedName;
    public int harvestQuantity = 1;
    public Sprite seedIcon;     // icon untuk inventory
    public Plant seedPrefab;    // prefab untuk ditanam lagi
    [Header("Seed Data")]
    public SeedData seedData;


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
    public bool IsReady() => (!dead && currentStage >= 2);

    public void OnWatered() => watered = true;
    public void OnFertilized() => fertilized = true;

    void Update()
    {
        if (dead) return;

        timeSincePlanted += Time.deltaTime;

        if (!watered && timeSincePlanted >= waterDeadline)
        {
            Die();
            return;
        }

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
                currentStage = 2;
                stageTimer = 0;
                UpdateSprite();
                soil?.OnPlantReady();
            }
        }
    }

    void UpdateSprite()
    {
        if (sr == null) return;

        if (currentStage == 0) sr.sprite = stage1;
        else if (currentStage == 1) sr.sprite = stage2;
        else sr.sprite = stage3;
    }

    public void Die()
    {
        dead = true;
        if (sr != null)
            sr.color = new Color(0.5f, 0.4f, 0.4f);
    }

    // dipanggil ketika ditanam agar stage normal lagi
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
