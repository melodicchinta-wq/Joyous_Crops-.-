using UnityEngine;

public class WaterRefill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerInteraction>();

            if (player != null && player.waterBar != null)
            {
                player.waterBar.RefillWater(100f);  // refill full
                Debug.Log("Air diisi ulang!");
            }
        }
    }
}
