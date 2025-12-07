using UnityEngine;

public class HouseDialogTrigger1 : MonoBehaviour
{
    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (triggered) return;
        if (col.CompareTag("Player"))
        {
            triggered = true;

            NPCDialog dialog = FindObjectOfType<NPCDialog>();
            dialog.StartHouseCutsceneDialog();
        }
    }
}
