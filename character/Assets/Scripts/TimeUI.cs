using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text dayText;

    void Update()
    {
        timeText.text = TimeController.instance.GetTimeString();
        dayText.text = "Day " + TimeController.instance.day;
    }
}
