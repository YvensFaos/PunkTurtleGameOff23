using UnityEngine;

public static class LayerHelper
{
    public static bool CheckLayer(LayerMask mask, int layer) => (mask.value & (1 << layer)) > 0;
}
