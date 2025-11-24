using UnityEngine;

public class PlayerToolVisual : MonoBehaviour
{
    public SpriteRenderer toolRenderer;
    public Sprite defaultHoeSprite; // sprite hoe di tangan pemain

    private bool showingTool = false;

    public void UpdateTool(ItemData tool)
    {
        if (tool == null)
        {
            toolRenderer.sprite = null;
            showingTool = false;
            return;
        }

        if (tool.itemName == "Hoe")
        {
            toolRenderer.sprite = defaultHoeSprite;
            showingTool = true;
        }
        else
        {
            toolRenderer.sprite = null;
            showingTool = false;
        }
    }

    public bool IsHoldingTool()
    {
        return showingTool;
    }
}
