using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class CurveHelper
    {
        [SerializeField]
        private AnimationCurve curve;
        [SerializeField]
        private List<Pair<float, float>> curvePoints;

        public void InitializeCurveHelper()
        {
            curve = new AnimationCurve();
            curvePoints.ForEach(point =>
            {
                curve.AddKey(point.One, point.Two);    
            });
        }

        public float Evaluate(float time) => curve.Evaluate(time);
    }
}
