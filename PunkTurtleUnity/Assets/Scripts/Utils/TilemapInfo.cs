using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

public class TilemapInfo : MonoBehaviour
{
    
    [SerializeField]
    private TilemapRenderer tilemapRenderer;

    [Button("Get info")]
    private void GetInfo()
    {
        var tilemapBounds = tilemapRenderer.bounds;

        // Access the dimensions of the bounds
        var width = tilemapBounds.size.x;
        var height = tilemapBounds.size.y;

        // Output the bounds and dimensions
        DebugUtils.DebugLogMsg("Tilemap Bounds: " + tilemapBounds);
        DebugUtils.DebugLogMsg("Width: " + width);
        DebugUtils.DebugLogMsg("Height: " + height);
    }
}
