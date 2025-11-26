using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;

    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private TileBase interactedTile; 
    private void Start()
    {
        foreach(var position in interactableMap.cellBounds.allPositionsWithin)
        {
            interactableMap.SetTile(position, hiddenInteractableTile);
        }
    }
    public bool IsInteractable(Vector3Int position)
    {
        if (interactableMap == null)
        {
            Debug.LogError("Interactable Map belum assign di inspector!");
            return false;
        }

        return interactableMap.HasTile(position);
    }
    public void SetInteracted(Vector3Int position)
    {
        interactableMap.SetTile(position, interactedTile);
    }
}
