using NaughtyAttributes;
using UnityEngine;

namespace Utils
{
    public class CurveHolder : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        protected float currentCurvePosition;
        [Space(10)]
        [SerializeField] 
        protected CurveHelper curve;
        [SerializeField] 
        protected float timer = 0.2f;

        private void Awake()
        {
            CalculateCurve();
        }

        [Button("Calculate Curve")]
        private void CalculateCurve()
        {
            curve.InitializeCurveHelper();
        }
    }
}