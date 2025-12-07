using UnityEngine;

public class PlaySoundOnEnter : MonoBehaviour
{
    private AudioSource source;

    [Header("Fade Settings")]
    public float fadeInSpeed = 1f;
    public float fadeOutSpeed = 1f;
    public float targetVolume = 1f;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.volume = 0f;   // mulai dari sunyi
        source.loop = true;   // biar musik terus
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            if (!source.isPlaying) source.Play();
            StartCoroutine(FadeIn());
            // FADE OUT BGM
            BGMFader.Instance.FadeOutBGM();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(FadeOut());
            // FADE IN BGM
            BGMFader.Instance.FadeInBGM();
        }
    }

    private System.Collections.IEnumerator FadeIn()
    {
        while (source.volume < targetVolume)
        {
            source.volume += Time.deltaTime * fadeInSpeed;
            yield return null;
        }
        source.volume = targetVolume;
    }

    private System.Collections.IEnumerator FadeOut()
    {
        while (source.volume > 0f)
        {
            source.volume -= Time.deltaTime * fadeOutSpeed;
            yield return null;
        }
        source.volume = 0f;
        source.Stop();
    }
}
