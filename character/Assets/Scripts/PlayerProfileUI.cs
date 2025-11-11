using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerProfileUI : MonoBehaviour
{
    public TMP_Text nameText;

    // Start is called before the first frame update
    void Start()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Pemain");
        nameText.text = playerName;
    }
}
