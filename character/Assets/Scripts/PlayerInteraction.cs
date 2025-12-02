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

    public WaterBar waterBar;
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
        // INTERAKSI UTAMA (E)
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentSoil == null) return;

            // ambil slot UI aktif
            int slotIndex = inventoryUI.selectedIndex;
            if (slotIndex >= 0 && slotIndex < inventoryUI.slots.Length)
            {
                var uiSlot = inventoryUI.slots[slotIndex];
                var slotSeed = uiSlot.GetCurrentSeed();

                if (slotSeed != null && slotSeed.amount > 0)
                {
                    if (currentSoil.currentState == SoilTile.SoilState.Hoed)
                    {
                        currentSoil.PlantSeed(slotSeed.plantPrefab);

                        inventory.UseActiveSeed(1);
                        inventoryUI.UpdateUI();

                        return;
                    }
                    else
                    {
                        Debug.Log("Tanah belum dicangkul!");
                        return;
                    }
                }
            }


            // 2. Kalau TIDAK ADA seed dipilih → CANGKUL
            if (inventory.hasHoe)
            {
                EquipItem("Hoe");
                currentSoil.Hoe();
            }
            else
            {
                Debug.Log("Kamu belum punya Hoe!");
            }
        }

        // AIR
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentSoil != null && waterBar.currentWater > 0)
            {
                currentSoil.Water();
                waterBar.UseWater(10f); // contoh pakai 10 air per tanah
            }
        }


        // PUPUK
        if (Input.GetKeyDown(KeyCode.P))
            currentSoil?.Fertilize();

        // PANEN
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (currentSoil != null)
            {
                var resultList = currentSoil.Harvest();

                foreach (var r in resultList)
                {
                    var newStacks = inventory.AddItemWithMax(r.name, r.qty, r.prefab, r.icon);

                    // tampilkan stack baru ke UI
                    foreach (var newSeed in newStacks)
                        inventoryUI.AddItemToEmptySlot(newSeed);
                }

                // update UI (qty, icon)
                inventoryUI.UpdateUI();
                inventoryUI.RefreshUIFromInventory();
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
