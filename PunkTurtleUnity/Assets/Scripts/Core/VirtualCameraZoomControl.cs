using Cinemachine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Utils;

namespace Core
{
    public class VirtualCameraZoomControl : CurveHolder
    {
        [SerializeField]
        private CinemachineVirtualCamera virtualCamera;
        
        private TweenerCore<float, float, FloatOptions> zoomTween;
        
        private void Start()
        {
            PlayerControl.GetSingleton().RegisterUpdateLinearValues(UpdateZoom);
        }
        
        private void OnDestroy()
        {
            PlayerControl.GetSingleton().UnregisterUpdateLinearValues(UpdateZoom);
        }

        private void UpdateZoom(float linear)
        {
            currentCurvePosition = curve.Evaluate(linear);
            zoomTween?.Kill();
            zoomTween = DOTween.To(() => virtualCamera.m_Lens.OrthographicSize,
                value => virtualCamera.m_Lens.OrthographicSize = value, 
                currentCurvePosition, timer);
        }
    }
}