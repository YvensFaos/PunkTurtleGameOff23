using Core;
using Lean.Pool;
using UnityEngine;
using Utils;

public class CollectableControl : AgnosticCollisionSolver2D
{
    [SerializeField] private int score;

    protected override void Solve(GameObject collidedWith)
    {
        if (!collidedWith.CompareTag("Player")) return;
        PlayerControl.GetSingleton().UpdateScore(score);
        LeanPool.Despawn(gameObject);
    }
}
