using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Portal tujuan")]
    public Portal destinationPortal;

    public float distance = 0.2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Biar tidak teleport bolak balik tak henti
        if (Vector2.Distance(transform.position, other.transform.position) > distance)
        {
            other.transform.position = destinationPortal.transform.position;
        }
    }
}
