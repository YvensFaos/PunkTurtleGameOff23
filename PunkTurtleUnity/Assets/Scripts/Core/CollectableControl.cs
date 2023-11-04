using Core;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using Utils;

public class CollectableControl : AgnosticCollisionSolver2D
{
    [SerializeField] private int score;
    
    private Tweener scaleTween;
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
        scaleTween = transform.DOShakeScale(0.2f, 0.09f).SetLoops(-1);
    }

    protected override void Solve(GameObject collidedWith)
    {
        if (!collidedWith.CompareTag("Player")) return;
        PlayerControl.GetSingleton().UpdateScore(score);
        LeanPool.Despawn(gameObject);
    }
}
