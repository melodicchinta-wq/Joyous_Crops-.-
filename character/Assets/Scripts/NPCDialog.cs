using UnityEngine;
using TMPro;

public class NPCDialog : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialogPanel;
    public TMP_Text dialogText;

    [Header("Player Inventory")]
    public Inventory playerInventory;

    private bool isPlayerNear = false;
    private int dialogIndex = 0;
    private bool hasGivenItems = false;

    [Header("Gift Seed Info")]
    public SeedData carrotSeed;   // wortel
    public SeedData onionSeed;

    string[] dialogLines =
    {
        "Hai! Kamu terlihat seperti ingin belajar bertani.",
        "Ambil cangkul tua ini. Gunakan dengan baik.",
        "Jangan lupa siram tanamanmu!"
    };

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
            ShowDialog();
    }

    private void ShowDialog()
    {
        dialogPanel.SetActive(true);
        dialogText.text = dialogLines[dialogIndex];
        dialogIndex++;

        if (dialogIndex >= dialogLines.Length && !hasGivenItems)
            GiveItemsToPlayer();
    }

    void GiveItemsToPlayer()
    {
        hasGivenItems = true;
        dialogPanel.SetActive(false);

        // Beri Hoe
        if (playerInventory != null)
            playerInventory.hasHoe = true;

        var hotbarSlots = FindObjectsOfType<HotbarSlotUI>();
        foreach (var slot in hotbarSlots)
        {
            if (slot.itemType == "Hoe")
                slot.ShowButton(true); // tampilkan tombol
        }


        InventoryUI ui = FindObjectOfType<InventoryUI>();
        // Tambahkan seed ke inventory
        playerInventory.AddQuantity(carrotSeed.seedName, 2, carrotSeed.prefab, carrotSeed.icon);
        playerInventory.AddQuantity(onionSeed.seedName, 2, onionSeed.prefab, onionSeed.icon);

        // Masukkan inventory ke UI (stack atau isi slot kosong)
        ui.ApplyInventoryAddsToSlots();

        ui.UpdateUI();



        Debug.Log("NPC memberi Hoe + Seed!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) isPlayerNear = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null || collision.gameObject == null) return;

        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;

            // pastikan dialogPanel masih ada sebelum disable
            if (dialogPanel != null)
                dialogPanel.SetActive(false);
        }
    }

}