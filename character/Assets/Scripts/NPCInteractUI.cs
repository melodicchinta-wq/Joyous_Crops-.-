using UnityEngine;

public class NPCInteractUI : MonoBehaviour
{
    public GameObject interactUI;   // assign InteractUI di inspector
    public string npcName = "NPC";

    public NPCDialog npcDialog;     // <-- TAMBAHAN: Referensi script dialog

    private bool playerNearby = false;

    void Start()
    {
        interactUI.SetActive(false);
    }

    void Update()
    {
        if (playerNearby)
        {
            // Tekan E atau F untuk mulai bicara
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F))
            {
                interactUI.SetActive(false);  // <-- HILANGKAN UI !!!

                if (npcDialog != null)
                {
                    npcDialog.StartDialogFromInteract(); // <-- MULAI DIALOG
                }
                else
                {
                    Debug.LogWarning("NPCDialog belum di-assign ke NPCInteractUI!");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;

            if (interactUI != null)
            {
                var label = interactUI.transform.Find("Label");

                if (label != null)
                {
                    var text = label.GetComponent<TMPro.TMP_Text>();
                    if (text != null)
                        text.text = "Talk with " + npcName;
                }

                interactUI.SetActive(true);
            }
            else
            {
                Debug.LogError("InteractUI belum di-assign di Inspector!");
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
            interactUI.SetActive(false);
        }
    }
}
