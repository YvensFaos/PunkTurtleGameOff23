using UnityEngine;

namespace Utils
{
    public static class TransformHelper
    {
        public static void ClearObjects(Transform transform)
        {
            var children = transform.childCount;
            for (var i = children - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
    }
}