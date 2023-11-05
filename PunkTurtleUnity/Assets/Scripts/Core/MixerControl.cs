using Core;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;
using Utils;

public class MixerControl : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private float currentCurvePosition;
    [SerializeField]
    private AudioMixer mixer;
    [SerializeField] 
    private CurveHelper pitchCurve;
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
        currentCurvePosition = pitchCurve.Evaluate(linear);
        zoomTween?.Kill();
        zoomTween = DOTween.To(() =>
            {
                mixer.GetFloat("SoundtrackPitch", out var value);
                return value;
            },
            value => mixer.SetFloat("SoundtrackPitch", value),
            currentCurvePosition, timer);
    }
    
    [Button("Calculate Curve")]
    private void CalculateCurve()
    {
        pitchCurve.InitializeCurveHelper();
    }
}
