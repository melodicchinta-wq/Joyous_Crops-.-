using System.Collections;
using UnityEngine;

public class RandomRain : MonoBehaviour
{
    public static RandomRain instance;

    public ParticleSystem rain;
    public float fadeInTime = 2f;

    private ParticleSystem.MainModule mainMod;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        mainMod = rain.main;
        mainMod.startColor = new Color(1, 1, 1, 0); //invisible awalnya
    }

    public void StartRainEffect()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInRain());
    }

    public void StopRainEffect()
    {
        StopAllCoroutines();
        rain.Stop();
    }

    IEnumerator FadeInRain()
    {
        rain.Play();
        float t = 0;
        while (t < fadeInTime)
        {
            while (t < fadeInTime)
            {
                t += Time.deltaTime;

                float a = Mathf.Lerp(0f, 1f, t / fadeInTime);
                mainMod.startColor = new Color(1, 1, 1, a);


                yield return null;
            }
        }
    }
}