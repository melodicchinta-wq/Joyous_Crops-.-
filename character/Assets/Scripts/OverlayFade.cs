using UnityEngine;
using UnityEngine.UI;

public class OverlayFade : MonoBehaviour
{
    public static OverlayFade instance;

    public Image overlayImage;
    public float maxDarkness = 0.75f;

    public int currentDay = 1;
    private float lastT = 0;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        float t = TimeController.instance.GetTimeNormalized();
        float alpha = CalculateDarkness(t);
        SetAlpha(alpha);

        DetectNewDay(t);
        lastT = t;
    }

    void DetectNewDay(float t)
    {
        // Pergantian day: t melewati titik reset (misal dari 0.99 → 0.0)
        if (lastT > 0.95f && t < 0.05f)
        {
            currentDay++;
            Debug.Log("DAY CHANGED → Day " + currentDay);
            WeatherManager.instance.OnNewDay(); // panggil weather
        }
    }

    float CalculateDarkness(float t)
    {
        float darkness = 0f;

        if (t >= 0.75f || t < 0.166f) darkness = 1f;
        else if (t >= 0.708f && t < 0.75f) darkness = Mathf.InverseLerp(0.708f, 0.75f, t);
        else if (t >= 0.166f && t < 0.25f) darkness = 1f - Mathf.InverseLerp(0.166f, 0.25f, t);
        else darkness = 0f;

        return darkness * maxDarkness;
    }

    void SetAlpha(float a)
    {
        Color c = overlayImage.color;
        c.a = a;
        overlayImage.color = c;
    }
}