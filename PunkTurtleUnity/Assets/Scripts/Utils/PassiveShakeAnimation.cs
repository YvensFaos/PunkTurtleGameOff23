using DG.Tweening;
using UnityEngine;

public class PassiveShakeAnimation : MonoBehaviour
{
    private Tweener scaleTween;
    [SerializeField]
    private float duration = 0.2f;
    [SerializeField]
    private float strength = 0.09f;
    
    private void Start()
    {
        StartAnimation();
    }

    private void OnEnable()
    {
        StartAnimation();
    }

    public void OnDisable()
    {
        scaleTween?.Kill();
    }

    protected void OnDestroy()
    {
        scaleTween?.Kill();
    }

    private void StartAnimation()
    {
        scaleTween?.Kill();
        scaleTween = transform.DOShakeScale(duration, strength).SetLoops(-1);
    }
}
