using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Image icon;
    public TMP_Text qtyText;
    public GameObject highlight;
    public Button button;

    private InventoryUI ui;
    private Inventory inventory;

    private SeedItem currentSeed;
    public int slotIndex;

    private Transform originalParent;
    private CanvasGroup cg;

    public bool IsEmpty => currentSeed == null;


    void Awake()
    {
        cg = icon.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = icon.gameObject.AddComponent<CanvasGroup>();

        // Pastikan sorting text di atas icon
        qtyText.canvas.overrideSorting = true;
        qtyText.canvas.sortingOrder = 20;

        icon.canvas.overrideSorting = true;
        icon.canvas.sortingOrder = 10;
    }


    public void Init(int index, InventoryUI uiRef)
    {
        slotIndex = index;
        ui = uiRef;
        inventory = uiRef.inventory;

        button.onClick.AddListener(() => ui.SelectSlot(index));
    }


    // =======================
    // SLOT CONTENT
    // =======================
    public void SetSlot(SeedItem seed)
    {
        currentSeed = seed;

        if (seed != null && seed.amount > 0)
        {
            icon.sprite = seed.icon;
            icon.gameObject.SetActive(true);
            qtyText.text = seed.amount.ToString();
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        currentSeed = null;
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        qtyText.text = "";
        highlight.SetActive(false);
    }

    public SeedItem GetCurrentSeed() => currentSeed;

    public void SetHighlight(bool value)
    {
        if (highlight != null)
            highlight.SetActive(value);
    }


    // =======================
    // DRAG SYSTEM
    // =======================
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsEmpty) return;

        originalParent = icon.transform.parent;
        icon.transform.SetParent(ui.transform);   // icon ke layer paling atas
        cg.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsEmpty) return;
        icon.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        icon.transform.SetParent(originalParent);
        icon.transform.localPosition = Vector3.zero;
        cg.blocksRaycasts = true;

        ui.UpdateUI();
        inventory.activeSeedIndex = ui.selectedIndex; // sync selection
    }


    // =======================
    // DROP SYSTEM (MERGE & SWAP)
    // =======================
    public void OnDrop(PointerEventData eventData)
    {
        var draggedSlot = eventData.pointerDrag.GetComponent<InventorySlotUI>();
        if (draggedSlot == null || draggedSlot == this) return;

        var draggedItem = draggedSlot.GetCurrentSeed();
        var targetItem = currentSeed;


        // ---- 1. DROP KE SLOT KOSONG → PINDAH ----
        if (targetItem == null)
        {
            SetSlot(draggedItem);
            draggedSlot.ClearSlot();
            ui.UpdateUI();
            return;
        }


        // ---- 2. MERGE STACK JIKA BISA ----
        if (inventory.TryMerge(targetItem, draggedItem))
        {
            if (draggedItem.amount <= 0)
                draggedSlot.ClearSlot();

            ui.RefreshUIFromInventory();
            return;
        }


        // ---- 3. TIDAK BISA MERGE → SWAP ----
        ui.SwapSlots(slotIndex, draggedSlot.slotIndex);
    }


    // Dipanggil ketika dipakai (misal menanam)
    public void UseItem(int count = 1)
    {
        if (IsEmpty) return;

        currentSeed.amount -= count;
        if (currentSeed.amount <= 0)
        {
            ClearSlot();
        }
        else
        {
            qtyText.text = currentSeed.amount.ToString();
        }
        inventory.DecreaseQuantityByName(currentSeed.seedName, count);
        ui.RefreshUIFromInventory();
    }
}
