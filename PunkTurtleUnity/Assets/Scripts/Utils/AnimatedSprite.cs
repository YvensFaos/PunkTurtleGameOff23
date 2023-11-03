using DG.Tweening;
using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class AnimatedSprite : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer sprite;
        [SerializeField] 
        private Transform rect;

        [Header("Animation Properties")] 
        [SerializeField]
        private float scale = 1.05f;
        [SerializeField]
        private float duration = 0.75f;

        private Tweener introTween;
        private Tweener bounceTween;
        
        private void Awake()
        {
            AssessUtils.CheckRequirement(ref sprite, this);
            AssessUtils.CheckRequirement(ref rect, this);
        }

        private void OnEnable()
        {
            var currentColor = sprite.color;
            var noAlpha = currentColor;
            noAlpha.a = 0.0f;
            sprite.color = noAlpha;
            introTween?.Kill();
            introTween = sprite.DOColor(currentColor, 0.2f).OnComplete(() =>
            {
                sprite.color = currentColor;
            });
            
            bounceTween?.Kill();
            bounceTween = rect.DOScale(new Vector3(scale,scale,scale), duration).SetLoops(-1).OnComplete(() =>
            {
                rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            });
        }

        private void OnDisable()
        {
            introTween?.Kill();
            bounceTween?.Kill();
        }
    }
}