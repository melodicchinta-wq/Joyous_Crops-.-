using UnityEngine;

public class Plant : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite stage1;
    public Sprite stage2;
    public Sprite stage3;

    [Header("Growth")]
    public float growTimePerStage = 5f; // waktu tahap (detik)
    public float waterDeadline = 7f;    // kalau tidak disiram dalam waktu ini => mati
    public float fertilizerSpeedMultiplier = 1.7f; // lebih cepat jika dipupuk

    [Header("Harvest")]
    public string seedName; // nama item yang akan ditambahkan ke inventory saat panen
    public int harvestQuantity = 1;

    private SpriteRenderer sr;
    private int currentStage = 0;
    private float stageTimer = 0f;
    private float timeSincePlanted = 0f;
    private bool watered = false;
    private bool fertilized = false;
    private bool dead = false;
    private SoilTile soil; // referensi ke tanah

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    public void SetSoil(SoilTile s)
    {
        soil = s;
    }

    public bool IsDead() => dead;
    public bool IsReady() => !dead && currentStage >= 2; // stage 2 = ready

    // dipanggil tanah saat disiram/pupuk agar plant tahu
    public void OnWatered()
    {
        watered = true;
        // reset waktu sejak planted sehingga deadline mulai dihitung dari tanam
        // (opsional: kita biarkan waterDeadline berlaku dari waktu tanam)
    }

    public void OnFertilized()
    {
        fertilized = true;
    }

    void Update()
    {
        if (dead) return;

        timeSincePlanted += Time.deltaTime;

        // belum disiram dan sudah melewati deadline -> mati
        if (!watered && timeSincePlanted >= waterDeadline)
        {
            Die();
            return;
        }

        // growth hanya berjalan jika sudah disiram setidaknya sekali
        if (watered)
        {
            float speed = fertilized ? fertilizerSpeedMultiplier : 1f;
            stageTimer += Time.deltaTime * speed;

            if (currentStage == 0 && stageTimer >= growTimePerStage)
            {
                currentStage = 1;
                stageTimer = 0f;
                UpdateSprite();
            }
            else if (currentStage == 1 && stageTimer >= growTimePerStage)
            {
                currentStage = 2;
                stageTimer = 0f;
                UpdateSprite();

                // beri tahu tanah bahwa tanaman sudah siap panen (opsional)
                if (soil != null) soil.OnPlantReady();
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
        // bisa ganti sprite jadi "dead" atau buat efek
        // untuk sekarang, biarkan tetap stage1/2 sprite tapi IsDead true
        // jika mau, set warna ke coklat
        if (sr != null) sr.color = new Color(0.5f, 0.4f, 0.4f);
    }

    // dipanggil saat player panen; kembalikan data panen
    public (string name, int qty) Harvest()
    {
        return (seedName, harvestQuantity);
    }

    // dipanggil saat player membuang tanaman mati (Destroy dipanggil di SoilTile)
}
