using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public LayerMask soilLayer;
    public float detectRadius = 0.5f;
    public Vector2 soilCheckOffset = new Vector2(0, -0.5f);

    private SoilTile currentSoil;
    private Inventory inventory;
    private InventoryUI inventoryUI;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        inventoryUI = FindObjectOfType<InventoryUI>();
    }

    void Update()
    {
        DetectSoil();

        // pilih slot seed
        if (Input.GetKeyDown(KeyCode.Alpha1)) inventory.SelectIndex(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) inventory.SelectIndex(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) inventory.SelectIndex(2);

        // cangkul
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!inventory.hasHoe)
            {
                Debug.Log("Kamu belum punya cangkul!");
                return;
            }
            currentSoil?.Hoe();
        }

        // tanam
        if (Input.GetKeyDown(KeyCode.E))
        {
            var seedItem = inventory.GetActiveSeedItem();

            if (seedItem != null && seedItem.amount > 0)
            {
                currentSoil?.PlantSeed(seedItem.plantPrefab);

                if (currentSoil != null && currentSoil.currentState == SoilTile.SoilState.Hoed)
                {
                    inventory.UseActiveSeed();
                }
            }

            inventoryUI.UpdateUI();
        }

        // air
        if (Input.GetKeyDown(KeyCode.Z))
            currentSoil?.Water();

        // pupuk
        if (Input.GetKeyDown(KeyCode.X))
            currentSoil?.Fertilize();

        // panen
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (currentSoil != null)
            {
                var resultList = currentSoil.Harvest();

                foreach (var r in resultList)
                {
                    inventory.AddQuantity(r.name, r.qty, r.prefab, r.icon);
                }

                inventoryUI.UpdateUI();

            }

        }
        if (Input.GetKeyDown(KeyCode.X))  // misal tombol X untuk cabut tanaman mati
            currentSoil?.RemoveDeadPlant();


        // reset tanah
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

                if (dist < minDistance)
                {
                    minDistance = dist;
                    closest = soil;
                }
            }
        }

        currentSoil = closest;
    }

}