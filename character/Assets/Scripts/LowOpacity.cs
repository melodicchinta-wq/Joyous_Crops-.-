using UnityEngine;

public class LowOpacity : MonoBehaviour
{
    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Color c = sr.color;
            c.a = 0.5f;      //set alpha aja
            sr.color = c;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Color c = sr.color;
            c.a = 1f;     //balikin ke full opacity
            sr.color = c;
        }
    }
}