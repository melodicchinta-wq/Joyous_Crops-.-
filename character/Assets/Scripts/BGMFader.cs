using UnityEngine;

public class BGMFader : MonoBehaviour
{
    public static BGMFader Instance;

    public AudioSource musicSource;
    public float fadeSpeed = 1f;

    private float targetVolume = 1f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (musicSource != null)
        {
            musicSource.volume = Mathf.MoveTowards(
                musicSource.volume,
                targetVolume,
                fadeSpeed * Time.deltaTime
            );
        }
    }

    public void FadeOutBGM()
    {
        targetVolume = 0f; // Jangan 0 biar masih terdengar halus
    }

    public void FadeInBGM()
    {
        targetVolume = 1f;
    }
}
