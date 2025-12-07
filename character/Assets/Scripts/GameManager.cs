using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public OverlayFade overlayFade;

    public static GameManager instance;

    public TileManager tileManager;
    private void Awake()
    {
        // Buat Singleton
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        tileManager = GetComponent<TileManager>();
    }
}
