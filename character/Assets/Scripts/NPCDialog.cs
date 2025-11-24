using UnityEngine;
using TMPro;

public class NPCDialog : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialogPanel;
    public TMP_Text dialogText;

    [Header("Player Inventory")]
    public Inventory playerInventory;   // <-- Drag Player Inventory di inspector

    private bool isPlayerNear = false;
    private int dialogIndex = 0;
    private bool hasGivenItems = false;

    [Header("Gift Seed Info")]
    public SeedData seedData;   // drag ScriptableObject seed di inspector

    string[] dialogLines =
    {
        "Hai! Kamu terlihat seperti ingin belajar bertani.",
        "Ambil cangkul tua ini. Gunakan dengan baik.",
        "Jangan lupa siram tanamanmu!"
    };

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            ShowDialog();
        }
    }

    private void ShowDialog()
    {
        dialogPanel.SetActive(true);
        dialogText.text = dialogLines[dialogIndex];

        dialogIndex++;

        // selesai dialog dan baru pertama kali give item
        if (dialogIndex >= dialogLines.Length && !hasGivenItems)
        {
            GiveItemsToPlayer();
        }
    }

    void GiveItemsToPlayer()
    {
        hasGivenItems = true;

        dialogPanel.SetActive(false);

        // Player menerima item dasar
        playerInventory.hasHoe = true;
        playerInventory.hasWaterBottle = true;

        // Player menerima bibit
        playerInventory.AddQuantity(
            seedData.seedName,
            1,
            seedData.prefab,
            seedData.icon
        );

        Debug.Log("NPC memberi item + seed.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null || collision.gameObject == null) return;

        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
            dialogPanel.SetActive(false);
        }
    }
}