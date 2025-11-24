using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public LayerMask soilLayer;
    public float detectRadius = 0.2f;
    public Vector2 soilCheckOffset = new Vector2(0, -0.25f);

    private SoilTile currentSoil;
    private Inventory inventory;
    private InventoryUI inventoryUI;
    public bool hasHoe = false; // awalnya false
    public string ActiveTool { get { return activeTool; } }

    // Tambahan untuk equip item ke player
    public GameObject hoeObject; // drag HandTool/Hoe di sini
    private string activeTool = "";

    // ============================================
    // EQUIP ITEM
    // ============================================
    public void EquipItem(string type)
    {
        hoeObject.SetActive(false);
        if (type == "Hoe")
        {
            hoeObject.SetActive(true);
            activeTool = "Hoe";
        }
        else activeTool = "";
    }

    // ============================================
    // UNITY LIFECYCLE
    // ============================================
    void Start()
    {
        inventory = GetComponent<Inventory>();
        inventoryUI = FindObjectOfType<InventoryUI>();
    }

    void Update()
    {
        DetectSoil();

        // pilih slot seed via keyboard
        if (Input.GetKeyDown(KeyCode.Alpha1)) inventory.SelectIndex(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) inventory.SelectIndex(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) inventory.SelectIndex(2);

        // HOE
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!inventory.hasHoe)
            {
                Debug.Log("Kamu belum punya cangkul!");
                return;
            }
            currentSoil?.Hoe();
        }

        // TANAM / HOE (tekan E)
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentSoil != null)
            {
                // Kalau player punya Hoe
                if (inventory.hasHoe)
                {
                    EquipItem("Hoe"); // tampilkan Hoe
                    currentSoil.Hoe();
                }

                // Tanam seed
                var seedItem = inventory.GetActiveSeedItem();
                if (seedItem != null && seedItem.amount > 0)
                {
                    currentSoil.PlantSeed(seedItem.plantPrefab);
                    if (currentSoil.currentState == SoilTile.SoilState.Hoed)
                        inventory.UseActiveSeed();
                }

                inventoryUI.UpdateUI();
            }
        }

        // AIR
        if (Input.GetKeyDown(KeyCode.Z))
            currentSoil?.Water();

        // PUPUK
        if (Input.GetKeyDown(KeyCode.X))
            currentSoil?.Fertilize();

        // PANEN
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (currentSoil != null)
            {
                var resultList = currentSoil.Harvest();
                foreach (var r in resultList)
                    inventory.AddQuantity(r.name, r.qty, r.prefab, r.icon);

                inventoryUI.UpdateUI();
            }
        }

        // CABUT TANAMAN MATI
        if (Input.GetKeyDown(KeyCode.X))
            currentSoil?.RemoveDeadPlant();

        // RESET TANAH
        if (Input.GetKeyDown(KeyCode.R))
            currentSoil?.ResetSoil();
    }

    void DetectSoil()
    {
        Vector2 checkPos = (Vector2)transform.position + soilCheckOffset;

        Collider2D[] hits = Physics2D.OverlapCircleAll(checkPos, detectRadius, soilLayer);

        float minDistance = float.MaxValue;
        SoilTile closest = null;

        foreach (var hit in hits)
        {
            SoilTile soil = hit.GetComponent<SoilTile>();
            if (soil != null)
            {
                float dist = Vector2.Distance(transform.position, soil.transform.position);
                if (dist < minDistance) closest = soil;
            }
        }

        currentSoil = closest;
    }

    // ============================================
    // GIZMOS UNTUK DEBUG
    // ============================================
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 checkPos = (Vector2)transform.position + soilCheckOffset;
        Gizmos.DrawWireSphere(checkPos, detectRadius);

        // Tambahkan indikator tanah yang terdeteksi
        if (currentSoil != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(currentSoil.transform.position, Vector3.one * 0.5f);
        }
    }
}
