using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "New Size", menuName = "Punk Turtle/Size", order = 0)]
    public class SizeSO : ScriptableObject
    {
        [SerializeField, Range(0.0f, 1.0f)]
        private float minTurtleSize;
        [SerializeField, Range(0.0f, 1.0f)]
        private float maxTurtleSize;

        public bool CanDamage(float currentLinearSize)
        {
            return currentLinearSize >= minTurtleSize && currentLinearSize <= maxTurtleSize;
        }
    }
}