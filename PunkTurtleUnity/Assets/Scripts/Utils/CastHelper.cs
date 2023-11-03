using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public static class CastHelper
    {
        public static IEnumerable<T> TryCastTo<T, S>(IEnumerable<S> originalList)
        {
            return originalList.Cast<T>();
        }
        
        public static IEnumerable<GameObject> GameObjectsFromRaycast2D(IEnumerable<RaycastHit2D> originalList)
        {
            return originalList.Select(raycast => raycast.collider.gameObject);
        }

        public static IEnumerable<T> CastToFromGameObject<T>(IEnumerable<GameObject> originalList)
        {
            var cast = new List<T>();
            foreach (var gameObject in originalList)
            {
                if (gameObject.TryGetComponent<T>(out T component))
                {
                    cast.Add(component);
                }
            }

            return cast;
        }
    }
}