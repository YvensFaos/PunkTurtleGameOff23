using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public static class AnimateTweenHelper
    {
        public static void Animate(ref RectTransform rectTransform, ref Tweener originalTween,float time, float shakeScaleStrength)
        {
            var rectCopy = rectTransform;
            if (originalTween != null)
            {
                originalTween.Kill();
                rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            originalTween = rectTransform.DOShakeScale(time, shakeScaleStrength).OnComplete(() =>
            {
                rectCopy.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            });
        }
    }
}