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
        PlayerControl.GetSingleton().RegisterGameOverEvent(OnGameOver);
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
        PlayerControl.GetSingleton().UnregisterGameOverEvent(OnGameOver);
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

    private void OnGameOver(int score, float distance)
    {
        var pitchDownTimer = 0.4f;
        zoomTween?.Kill();
#if UNITY_WEBGL
        zoomTween = source.DOPitch(0.0f, pitchDownTimer);
#else
        zoomTween = DOTween.To(() =>
            {
                mixer.GetFloat("SoundtrackPitch", out var value);
                return value;
            },
            value => mixer.SetFloat("SoundtrackPitch", value),
            0.0f, pitchDownTimer);
#endif
    }
}
