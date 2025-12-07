using UnityEngine;

public class ChestInteract : MonoBehaviour
{
    public GameObject chestPanel;   // panel UI untuk chest
    public GameObject interactHint; // tulisan "Press E to open"

    private bool isPlayerNear = false;
    private bool isOpen = false;

    void Start()
    {
        if (chestPanel != null)
            chestPanel.SetActive(false);

        if (interactHint != null)
            interactHint.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            ToggleChest();
        }
    }

    private void ToggleChest()
    {
        isOpen = !isOpen;
        chestPanel.SetActive(isOpen);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            isPlayerNear = true;
            if (interactHint != null)
                interactHint.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (interactHint != null)
                interactHint.SetActive(false);

            chestPanel.SetActive(false);
            isOpen = false;
        }
    }
}
