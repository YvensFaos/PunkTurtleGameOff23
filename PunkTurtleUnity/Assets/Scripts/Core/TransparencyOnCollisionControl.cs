using DG.Tweening;
using UnityEngine;
using Utils;

public class TransparencyOnCollisionControl : AgnosticCollisionSolver2D
{
    [SerializeField]
    private SpriteRenderer transparencyRenderer;
    [SerializeField]
    private float transparency = 0.5f;
    
    private Color rendererOriginalColor;
    private Color rendererAlphaColor;
    private Tweener colorTween;
    
    private void Awake()
    {
        AssessUtils.CheckRequirement(ref transparencyRenderer, this);

        rendererOriginalColor = transparencyRenderer.color;
        rendererAlphaColor = transparencyRenderer.color;
        rendererAlphaColor.a = transparency;
    }

    protected override void Solve(GameObject collidedWith)
    {
        if (!collidedWith.CompareTag("Player")) return;
        colorTween?.Kill();
        colorTween = transparencyRenderer.DOColor(rendererAlphaColor, 0.4f);
    }

    protected override void SolveExit(GameObject exitWith)
    {
        if (!exitWith.CompareTag("Player")) return;
        colorTween?.Kill();
        colorTween = transparencyRenderer.DOColor(rendererOriginalColor, 0.4f);
    }
}
