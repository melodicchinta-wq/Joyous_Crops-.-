using UnityEngine;
using UnityEngine.UI;

public class HotbarSlotUI : MonoBehaviour
{
    public Button button;       // drag Button prefab
    public Sprite hoeSprite;    // sprite Hoe
    public string itemType;     // "Hoe" atau lainnya

    void Start()
    {
        button.onClick.AddListener(OnClickSlot);
        button.gameObject.SetActive(false); // hide button di awal
    }

    public void ShowButton(bool show)
    {
        button.gameObject.SetActive(show);
        if (show && itemType == "Hoe" && hoeSprite != null)
        {
            button.image.sprite = hoeSprite;
        }
    }

    void OnClickSlot()
    {
        var player = FindObjectOfType<PlayerInteraction>();
        if (player != null)
        {
            if (player.ActiveTool == itemType)
                player.EquipItem(""); // hide tool
            else
                player.EquipItem(itemType); // tampilkan tool
        }
    }
}
