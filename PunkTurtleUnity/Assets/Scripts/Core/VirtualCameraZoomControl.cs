using Cinemachine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using NaughtyAttributes;
using UnityEngine;
using Utils;

namespace Core
{
    public class VirtualCameraZoomControl : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private float currentCurvePosition;
        [Space(10)]
        [SerializeField]
        private CinemachineVirtualCamera virtualCamera;
        [SerializeField] 
        private CurveHelper distanceCurve;
        [SerializeField] 
        private float timer = 0.2f;
        
        private TweenerCore<float, float, FloatOptions> zoomTween;
        
        private void Start()
        {
            PlayerControl.GetSingleton().RegisterUpdateLinearValues(UpdateZoom);
            CalculateCurve();
        }
        
        private void OnDestroy()
        {
            PlayerControl.GetSingleton().UnregisterUpdateLinearValues(UpdateZoom);
        }

        private void UpdateZoom(float linear)
        {
            currentCurvePosition = distanceCurve.Evaluate(linear);
            zoomTween?.Kill();
            zoomTween = DOTween.To(() => virtualCamera.m_Lens.OrthographicSize,
                value => virtualCamera.m_Lens.OrthographicSize = value, 
                currentCurvePosition, timer);
        }

        [Button("Calculate Curve")]
        private void CalculateCurve()
        {
            distanceCurve.InitializeCurveHelper();
        }
    }
}