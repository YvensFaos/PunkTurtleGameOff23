using Core;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Audio;
using Utils;

public class MixerControl : CurveHolder
{
    [SerializeField]
    private AudioMixer mixer;
    [SerializeField] 
    private AudioSource source;
    
    private TweenerCore<float, float, FloatOptions> zoomTween;
    
    private void Start()
    {
        PlayerControl.GetSingleton().RegisterUpdateLinearValues(UpdateMixer);
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
        currentCurvePosition = curve.Evaluate(linear);
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
}
