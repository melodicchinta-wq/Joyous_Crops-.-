using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class NPCDialog : MonoBehaviour
{
    private bool isCutscene = false;

    [Header("UI")]
    public GameObject dialogPanel;
    public TMP_Text dialogText;

    [Header("Choices UI")]
    public GameObject choicesPanel;
    public Button choiceButton1;
    public Button choiceButton2;

    [Header("Player Inventory")]
    public Inventory playerInventory;

    [Header("Gift Seed Info")]
    public SeedData carrotSeed;
    public SeedData onionSeed;

    private bool isPlayerNear = false;
    private bool hasGivenItems = false;
    private bool isTalking = false;

    AudioManager audioManager;
    public void StartDialogFromInteract()
    {
        StartDialog();   // panggil dialog utama
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        // buka dialog hanya jika belum bicara
        if (isPlayerNear && !isTalking && Input.GetKeyDown(KeyCode.F))
        {
            StartDialog();
        }
    }

    void StartDialog()
    {
        dialogPanel.SetActive(true);
        isTalking = true;

        ShowNPCLine_Intro();
    }

    // ——————————————————————————
    // 1. BAGIAN DIALOG
    // ——————————————————————————
    void ShowNPCLine_Intro()
    {
        dialogText.text = "Hei! Kamu bukan penduduk sini, kan? Kamu terlihat kebingungan.";

        ShowChoices(
            "Iya, aku tiba tiba ada di ujung sana",
            "Iya, dan tidak, aku cuma lewat."
        );
    }

    void ShowNPCLine_AfterYes()
    {
        dialogText.text = "Tinggal aja di sana. Nih, cangkul dan benih biar nggak nganggur.";

        ShowChoices(
            "Makasih banyak!",
            "Apa ada hal lain yang mungkin harus aku tahu?"
        );
    }

    void ShowNPCLine_GiveInfo()
    {
        dialogText.text = "Jangan lupa jalan jalan di desa ini ya kalo kamu mulai bosan";

        ShowChoices(
            "Baik, terima kasih!",
            "Oke!"
        );
    }

    // ——————————————————————————
    // 2. MENAMPILKAN PILIHAN PLAYER
    // ——————————————————————————
    void ShowChoices(string option1, string option2)
    {
        if (isCutscene)
        {
            // Jangan tampilkan choices saat cutscene
            choicesPanel.SetActive(false);
            return;
        }

        choicesPanel.SetActive(true);

        choiceButton1.GetComponentInChildren<TMP_Text>().text = option1;
        choiceButton2.GetComponentInChildren<TMP_Text>().text = option2;

        choiceButton1.onClick.RemoveAllListeners();
        choiceButton2.onClick.RemoveAllListeners();

        choiceButton1.onClick.AddListener(() => PlayerChoose(option1));
        choiceButton2.onClick.AddListener(() => PlayerChoose(option2));
    }

    // ——————————————————————————
    // 3. PLAYER PILIH JAWABAN
    // ——————————————————————————
    void PlayerChoose(string choice)
    {
        choicesPanel.SetActive(false);

        if (choice.Contains("Iya"))
        {
            ShowNPCLine_AfterYes();
        }
        else if (choice.Contains("lewat"))
        {
            dialogText.text = "Baiklah, hati-hati di jalan!";
            Invoke(nameof(CloseDialog), 1.5f);
        }
        else if (choice.Contains("Makasih"))
        {
            ShowNPCLine_GiveInfo();
        }
        else if (choice.Contains("hal lain"))
        {
            ShowNPCLine_GiveInfo();
        }
        else
        {
            dialogText.text = "Semoga berhasil!";
            Invoke(nameof(EndDialogAndGiveItems), 1.5f);
        }
    }

    // ——————————————————————————
    // 4. MENUTUP DIALOG
    // ——————————————————————————
    void CloseDialog()
    {
        dialogPanel.SetActive(false);
        isTalking = false;
    }

    // ——————————————————————————
    // 5. GIVE ITEMS (HOE + SEED)
    // ——————————————————————————
    void EndDialogAndGiveItems()
    {
        if (!hasGivenItems)
        {
            GiveItemsToPlayer();
        }

        CloseDialog();
    }

    void GiveItemsToPlayer()
    {
        hasGivenItems = true;

        // kasih hoe
        if (playerInventory != null)
            playerInventory.hasHoe = true;

        // hotbar update
        var hotbarSlots = FindObjectsOfType<HotbarSlotUI>();
        foreach (var slot in hotbarSlots)
        {
            if (slot.itemType == "Hoe")
                slot.ShowButton(true);
        }

        // tambah seed
        InventoryUI ui = FindObjectOfType<InventoryUI>();
        playerInventory.AddQuantity(carrotSeed.seedName, 2, carrotSeed.prefab, carrotSeed.icon);
        playerInventory.AddQuantity(onionSeed.seedName, 2, onionSeed.prefab, onionSeed.icon);

        ui.ApplyInventoryAddsToSlots();
        ui.SyncInventoryWithUI();
        ui.UpdateUI();

        audioManager.PlaySFX(audioManager.feedback);

        Debug.Log("NPC memberi Hoe + Seed!");
    }

    // ——————————————————————————
    // 6. TRIGGER AREA
    // ——————————————————————————
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) isPlayerNear = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
            CloseDialog();
        }
    }
    public void StartIntroCutsceneDialog(System.Action onFinish)
    {
        isCutscene = true;                // ← mode cutscene
        choicesPanel.SetActive(false);    // ← tombol disembunyikan

        dialogPanel.SetActive(true);
        isTalking = true;

        StartCoroutine(IntroCutscene(onFinish));
    }

    IEnumerator IntroCutscene(System.Action onFinish)
    {
        // Isi dialog pembuka di sini!
        string[] introLines =
        {
        "Hah? Ini di mana? jelas ini bukan di pendakian kegiatan acaraku",
        "Siapa yang mindahin aku ke sini? Maling? Alien?",
        "Apa yang terjadi semalam…",
        "Ya ampun, memory-ku lemot lagi…",
    };

        foreach (string line in introLines)
        {
            dialogText.text = line;
            yield return new WaitForSeconds(1.8f);
        }

        dialogPanel.SetActive(false);
        isTalking = false;

        isCutscene = false;        // ← mode cutscene berakhir
        onFinish?.Invoke();   // aktifkan kembali player
    }
    public void StartMonolog(string[] monologLines)
    {
        StartCoroutine(MonologCoroutine(monologLines));
    }

    IEnumerator MonologCoroutine(string[] lines)
    {
        isTalking = true;
        dialogPanel.SetActive(true);

        // sembunyikan tombol
        choicesPanel.SetActive(false);

        // disable player movement (jika ada scriptnya)
        var playerMove = FindObjectOfType<CharacterController>();
        if (playerMove != null) playerMove.enabled = false;

        foreach (string line in lines)
        {
            dialogText.text = line;
            yield return new WaitForSeconds(2f);
        }

        dialogPanel.SetActive(false);
        isTalking = false;

        // aktifkan kembali movement
        if (playerMove != null) playerMove.enabled = true;
    }
    public void StartHouseCutsceneDialog()
    {
        StartCoroutine(HouseCutscene());
    }
    IEnumerator HouseCutscene()
    {
        isCutscene = true;
        isTalking = true;
        dialogPanel.SetActive(true);
        choicesPanel.SetActive(false);   // jangan tampilkan pilihan

        string[] lines =
        {
        "Eh? Rumah siapa ni…? Kok kayak rumah gratis siap pakai…",
        "Hmm… kalau nggak ada yang punya… boleh la yaa sementara jadi rumahku dulu.",
        "Eh eh ini ladang luas juga ya… tapi kosong. Persis kaya dompetku di dunia sebelumnya.",
        "Kayaknya kalau aku tanem sesuatu, bisa mulai hidup baru di sini.",
        "Atau minimal… bisa nanem harapan.",
        "Oke oke… sebelum aku resmi mengklaim rumah ini, aku harus cari orang dulu…",
        "Semoga aja ketemu orang baik."  ,
        };

        foreach (string line in lines)
        {
            dialogText.text = line;
            yield return new WaitForSeconds(2f);
        }

        // selesai
        dialogPanel.SetActive(false);
        isTalking = false;
        isCutscene = false;
    }

}
