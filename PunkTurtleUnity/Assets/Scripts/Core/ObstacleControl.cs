using System.Collections;
using UnityEngine;
using Utils;

namespace Core
{
    public class ObstacleControl : AgnosticCollisionSolver2D
    {
        [SerializeField]
        private float cooldownTimer = 4.0f;
        
        private bool cooldown;
        
        protected override void Solve(GameObject collidedWith)
        {
            if (cooldown || !collidedWith.CompareTag("Player")) return;
            PlayerControl.GetSingleton().GetHit();
            cooldown = true;
            StartCoroutine(ObstacleCooldown());
        }

        private IEnumerator ObstacleCooldown()
        {
            yield return new WaitForSeconds(cooldownTimer);
            cooldown = false;
        }
    }
}