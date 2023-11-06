using Core;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Utils;

public class BorderControl : CurveHolder
{
    private TweenerCore<float, float, FloatOptions> moveTween;
    
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
        moveTween?.Kill();
        moveTween = DOTween.To(() => transform.localPosition.x,
            value =>
            {
                var localPosition = transform.localPosition;
                localPosition.x = value;
                transform.localPosition = localPosition;
            },
            currentCurvePosition, timer);
    }
}
