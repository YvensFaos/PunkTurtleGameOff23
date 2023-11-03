using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public static class AnimateMaterialProperty
    {
        public static Tweener AnimateProperty<T>(Material animateMaterial, string property, T value, float time,
            UnityAction callback = null)
        {
            return AnimateProperty(animateMaterial, Shader.PropertyToID(property), value, time, callback);
        }
        
        public static Tweener AnimateProperty<T>(Material animateMaterial, int property, T value, float time, UnityAction callback = null)
        {
            switch (value)
            {
                case int intT:
                {
                    return DOTween.To(() => animateMaterial.GetInt(property), value => animateMaterial.SetInt(property, (int)value), intT, time)
                        .OnComplete(
                            () =>
                            {
                                callback?.Invoke();
                            });
                }
                case float floatT:
                {
                    return DOTween.To(() => animateMaterial.GetFloat(property), value => animateMaterial.SetFloat(property, value), floatT, time)
                        .OnComplete(
                            () =>
                            {
                                callback?.Invoke();
                            });
                }
                case Vector3 vector3T:
                {
                    var asVector4 = new Vector4(vector3T.x, vector3T.y, vector3T.z, 0.0f);
                    return animateMaterial.DOVector(asVector4, property, time).OnComplete(() =>
                    {
                        callback?.Invoke();
                    });
                }
                case Vector4 vector4T:
                {
                    return animateMaterial.DOVector(vector4T, property, time).OnComplete(() =>
                    {
                        callback?.Invoke();
                    });
                }
                default:
                    //TODO throw some exception here
                    return null;
            }
        }

        public static Tweener AnimateIntPropertyWithCurve(Material animateMaterial, int property, float time,
            AnimationCurve curve, bool inverse = false, UnityAction callback = null)
        {
            var curveLerp = inverse ? 1.0f : 0.0f;
            return DOTween.To(() => curveLerp,
                    value =>
                    {
                        curveLerp = value;
                        animateMaterial.SetInt(property, (int)curve.Evaluate(curveLerp));
                    }, inverse ? 0.0f : 1.0f, time)
                .OnComplete(() => { callback?.Invoke(); });
        }
        
        public static Tweener AnimateFloatPropertyWithCurve(Material animateMaterial, int property, float time,
            AnimationCurve curve, bool inverse = false, UnityAction callback = null)
        {
            var curveLerp = inverse ? 1.0f : 0.0f;
            return DOTween.To(() => curveLerp,
                    value =>
                    {
                        DebugUtils.DebugLogMsg($"Curve Lerp ? {curveLerp} : {curve.Evaluate(curveLerp)}");
                        curveLerp = value;
                        animateMaterial.SetFloat(property, curve.Evaluate(curveLerp));
                    }, inverse ? 0.0f : 1.0f, time)
                .OnComplete(() => { callback?.Invoke(); });
        }
    }
}