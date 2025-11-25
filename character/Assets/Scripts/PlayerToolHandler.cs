using UnityEngine;

public class PlayerToolHandler : MonoBehaviour
{
    public SpriteRenderer handTool;
    public Sprite hoeSprite;

    public void ToggleHoe()
    {
        if (handTool.enabled && handTool.sprite == hoeSprite)
            handTool.enabled = false;
        else
        {
            handTool.sprite = hoeSprite;
            handTool.enabled = true;
        }
    }
}