using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Soil Detection")]
    public LayerMask soilLayer;
    public float detectRadius = 0.35f;
    public Vector2 soilCheckOffset = new Vector2(0, -0.5f);

    private SoilTile currentSoil;
    private Inventory inventory;

    void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    void Update()
    {
        DetectSoil();

        // pilih bibit 1..n (kamu bisa tambah jumlah sesuai inventory)
        if (Input.GetKeyDown(KeyCode.Alpha1)) inventory.SelectIndex(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) inventory.SelectIndex(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) inventory.SelectIndex(2);
        if (Input.GetKeyDown(KeyCode.Alpha0)) inventory.SelectIndex(-1);

        // Cangkul
        if (Input.GetKeyDown(KeyCode.Q))
            currentSoil?.Hoe();

        // Tanam (pakai inventory)
        if (Input.GetKeyDown(KeyCode.E))
        {
            var seedItem = inventory.GetActiveSeedItem();
            if (seedItem != null && seedItem.amount > 0)
            {
                currentSoil?.PlantSeed(seedItem.plantPrefab);
                if (currentSoil != null && currentSoil.currentState == SoilTile.SoilState.Hoed)
                    inventory.UseActiveSeed();
            }
        }

        // Siram
        if (Input.GetKeyDown(KeyCode.Z))
        {
            currentSoil?.Water();
        }

        // Pupuk
        if (Input.GetKeyDown(KeyCode.X))
        {
            currentSoil?.Fertilize();
        }

        // Panen — panen menambah inventory
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (currentSoil != null)
            {
                var result = currentSoil.Harvest();
                if (!string.IsNullOrEmpty(result.name) && result.qty > 0)
                {
                    // Tambahkan keuntungan panen
                    int bonus = UnityEngine.Random.Range(1, 3); // dapat 1 atau 2 tambahan
                    int totalQty = result.qty + bonus;

                    inventory.AddQuantity(result.name, totalQty);
                    Debug.Log($"Panen {result.name} +{totalQty} (bonus {bonus})");
                }
                else
                {
                    Debug.Log("Tidak bisa panen (belum siap atau mati)");
                }
            }
        }


        // Buang tanaman mati (jika ada)
        if (Input.GetKeyDown(KeyCode.B))
        {
            currentSoil?.RemoveDeadPlant();
        }

        // Reset tanah ke hijau
        if (Input.GetKeyDown(KeyCode.R))
            currentSoil?.ResetSoil();
    }

    void DetectSoil()
    {
        Vector2 checkPos = (Vector2)transform.position + soilCheckOffset;
        Collider2D hit = Physics2D.OverlapCircle(checkPos, detectRadius, soilLayer);
        currentSoil = hit ? hit.GetComponent<SoilTile>() : null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 checkPos = (Vector2)transform.position + soilCheckOffset;
        Gizmos.DrawWireSphere(checkPos, detectRadius);
    }
}
