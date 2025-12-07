using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterCustomization : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerNameInput;

    public void SaveSelection()
    {
        string playerName = playerNameInput.text;
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Pemain";
        }

        // SIMPAN NAMA
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();
    }

    public void GoToNextScene(string sceneName)
    {
        SaveSelection();
        SceneManager.LoadScene(sceneName);
    }
}
