using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Image icon;
    public TMP_Text qtyText;
    public GameObject highlight;
    public Button button;

    private InventoryUI ui;
    private SeedItem currentSeed;

    private Transform originalParent;
    private CanvasGroup canvasGroup;
    public bool IsEmpty => currentSeed == null;


    void Awake()
    {
        canvasGroup = icon.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = icon.gameObject.AddComponent<CanvasGroup>();
    }

    public void Init(int index, InventoryUI uiRef)
    {
        ui = uiRef;
        button.onClick.AddListener(() => ui.SelectSlot(index));
    }

    public void SetSlot(SeedItem seed)
    {
        currentSeed = seed;
        if (seed != null && seed.amount > 0)
        {
            icon.gameObject.SetActive(true);
            icon.sprite = seed.icon;
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
        icon.gameObject.SetActive(false);
        qtyText.text = "";
        highlight.SetActive(false);
    }

    public void SetHighlight(bool value)
    {
        if (highlight != null)
            highlight.SetActive(value);
    }

    public SeedItem GetCurrentSeed()
    {
        return currentSeed;
    }

    // ======================
    // DRAG ICON
    // ======================
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentSeed == null) return;

        originalParent = icon.transform.parent;
        icon.transform.SetParent(ui.transform); // tarik ke atas semua UI
        canvasGroup.blocksRaycasts = false;      // supaya slot bisa detect drop
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentSeed == null) return;

        icon.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        icon.transform.SetParent(originalParent);
        icon.transform.localPosition = Vector3.zero;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlotUI draggedSlot = eventData.pointerDrag.GetComponentInParent<InventorySlotUI>();
        if (draggedSlot == null) return;

        // swap item
        SeedItem temp = draggedSlot.currentSeed;
        draggedSlot.SetSlot(currentSeed);
        SetSlot(temp);

        // sinkronisasi ke Inventory
        ui.SyncInventoryWithUI();
    }
    public void UseItem(int count = 1)
    {
        if (currentSeed == null) return;

        currentSeed.amount -= count;
        if (currentSeed.amount <= 0)
            ClearSlot(); // slot jadi kosong

        // update text
        qtyText.text = currentSeed != null ? currentSeed.amount.ToString() : "";
    }

}
