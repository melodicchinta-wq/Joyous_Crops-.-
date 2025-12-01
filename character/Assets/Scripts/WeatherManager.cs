using UnityEngine;

public enum WeatherType { Sunny, Rain }

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager instance;

    public WeatherType todayWeather;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        OnNewDay();
    }

    public void OnNewDay()
    {
        float r = Random.value;

        if (r < 0.4f)
            todayWeather = WeatherType.Rain;
        else
            todayWeather = WeatherType.Sunny;

        Debug.Log("Weather Day " + OverlayFade.instance.currentDay + ": " + todayWeather);

        if (todayWeather == WeatherType.Rain)
            RandomRain.instance.StartRainEffect();
        else
            RandomRain.instance.StopRainEffect();
    }
}