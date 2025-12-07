using System.Collections;
using UnityEngine;

public class RandomRain : MonoBehaviour
{
    public int currentDay = 1;

    public static RandomRain instance;

    public ParticleSystem rain;
    public float fadeInTime = 2f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (rain == null)
        {
            Debug.LogError("RAIN PARTICLE BELUM DI-ASSIGN!!");
            return;
        }

        // ambil main module saat needed, bukan disimpan
        var m = rain.main;
        m.startColor = new Color(1, 1, 1, 0);
    }

    public void StartRainEffect()
    {
        // ❗ Hari 1 harus sunny → tidak boleh hujan
        if (currentDay == 1)
        {
            Debug.Log("Day 1: Sunny. Hujan dimatikan.");
            return;
        }
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
            t += Time.deltaTime;
            float a = Mathf.Lerp(0f, 1f, t / fadeInTime);

            var m = rain.main;              // ← ambil ulang setiap frame
            m.startColor = new Color(1, 1, 1, a);

            yield return null;
        }
    }
}
