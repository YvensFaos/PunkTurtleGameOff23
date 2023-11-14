using Core;
using DG.Tweening;
using UnityEngine;
using Utils;

public class CollectableControl : AgnosticCollisionSolver2D
{
    [SerializeField] private int score;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool collected;

    private void Awake()
    {
        AssessUtils.CheckRequirement(ref spriteRenderer, this);
    }

    private void OnEnable()
    {
        collected = false;
    }

    protected override void Solve(GameObject collidedWith)
    {
        if (collected) return;
        if (!collidedWith.CompareTag("Player")) return;
        var player = PlayerControl.GetSingleton();
        collected = true;
        var finalColor = Color.white;
        finalColor.a = 0.0f;
        spriteRenderer.DOColor(finalColor, 0.22f);
        transform.DOMove(player.transform.position, 0.25f).OnComplete(() =>
        {
            player.UpdateScore(score);
            // LeanPool.Despawn(gameObject);
            Destroy(gameObject);
        });
    }
}
