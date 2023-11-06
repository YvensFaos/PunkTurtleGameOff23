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
    private AudioSource source;
    [SerializeField] 
    private CurveHelper pitchCurve;
    [SerializeField] 
    private float timer = 0.2f;
    
    private TweenerCore<float, float, FloatOptions> zoomTween;
    
    private void Start()
    {
        PlayerControl.GetSingleton().RegisterUpdateLinearValues(UpdateMixer);
        CalculateCurve();
     
        //Resets to 1.0f
#if UNITY_WEBGL
        source.pitch = 1.0f;
#else
        mixer.SetFloat("SoundtrackPitch", 1.0f);
#endif
    }
        
    private void OnDestroy()
    {
        PlayerControl.GetSingleton().UnregisterUpdateLinearValues(UpdateMixer);
    }

    private void UpdateMixer(float linear)
    {
        currentCurvePosition = pitchCurve.Evaluate(linear);
        zoomTween?.Kill();
#if UNITY_WEBGL
        zoomTween = source.DOPitch(currentCurvePosition, timer);
#else
        zoomTween = DOTween.To(() =>
            {
                mixer.GetFloat("SoundtrackPitch", out var value);
                return value;
            },
            value => mixer.SetFloat("SoundtrackPitch", value),
            currentCurvePosition, timer);
#endif
    }
    
    [Button("Calculate Curve")]
    private void CalculateCurve()
    {
        pitchCurve.InitializeCurveHelper();
    }
}
