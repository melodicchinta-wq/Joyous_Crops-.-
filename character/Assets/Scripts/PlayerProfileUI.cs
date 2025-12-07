using UnityEngine;
using TMPro;

public class PlayerProfileUI : MonoBehaviour
{
    public TMP_Text nameText;

    void Start()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Pemain");
        nameText.text = playerName;
    }
}
