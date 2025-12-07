using UnityEngine;

public class IndoorArea : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RandomRain.instance.StopRainEffect();   // matiin hujan
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RandomRain.instance.StartRainEffect();  // hidupkan lagi
        }
    }
}
