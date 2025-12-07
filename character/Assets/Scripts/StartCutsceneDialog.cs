using System.Collections;
using UnityEngine;

public class StartCutsceneDialog : MonoBehaviour
{
    public NPCDialog dialogNPC;         // drag NPCDialog yg kamu mau pakai
    public CharacterController player;     // drag script movement player
    public float delayBeforeStart = 0.5f;

    void Start()
    {
        StartCoroutine(BeginStartDialog());
    }

    IEnumerator BeginStartDialog()
    {
        // Nonaktifkan gerak player
        if (player != null)
            player.enabled = false;

        yield return new WaitForSeconds(delayBeforeStart);

        // Mulai dialog awal
        dialogNPC.StartIntroCutsceneDialog(() =>
        {
            // Callback setelah dialog selesai → aktifkan gerak player
            if (player != null)
                player.enabled = true;
        });
    }
}
