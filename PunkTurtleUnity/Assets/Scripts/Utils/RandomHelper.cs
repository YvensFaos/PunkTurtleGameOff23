using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class RandomHelper<T>
    {
        public static T GetRandomFromList(List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
    }

    public static class RandomChanceUtils
    {
        public static bool GetChance(float upTo, float max = 100.0f)
        {
            return Random.Range(0, max) <= upTo;
        }
        
        public static float GetRandomInRange(Vector2 range)
        {
            return Random.Range(range.x, range.y);
        }

        public static int GetRandomInRangeAsInt(Vector2 range)
        {
            return (int)GetRandomInRange(range);
        }
    }

    public static class RandomPointUtils
    {
        public static Vector3 GetRandomPointWithBox(BoxCollider boxCollider)
        {
            var bounds = boxCollider.bounds;
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z));
        }

        public static Vector3 GetRandomPointWithinCircleCollider2(CircleCollider2D circleCollider)
        {
            var randomPoint2D = (Vector2)circleCollider.bounds.center + Random.insideUnitCircle * circleCollider.radius;
            return new Vector3(randomPoint2D.x, randomPoint2D.y, 0.0f);
        }
    }
}