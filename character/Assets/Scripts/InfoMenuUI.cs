using UnityEngine;

public class InfoMenuUI : MonoBehaviour
{
    public GameObject infoPanel; // Panel InfoMenu

    void Start()
    {
        // Info menu muncul di awal
        infoPanel.SetActive(true);

        // Freeze player movement selama info muncul
        Time.timeScale = 0;
    }

    public void CloseInfo()
    {
        infoPanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void OpenInfo() 
    { 
        infoPanel.SetActive(true); 
        Time.timeScale = 0; 
    }
}
