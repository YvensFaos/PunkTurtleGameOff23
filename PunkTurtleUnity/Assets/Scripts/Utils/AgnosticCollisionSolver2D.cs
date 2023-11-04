using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class AgnosticCollisionSolver2D : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Solve(other.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Solve(other.gameObject);
        }

        protected abstract void Solve(GameObject collidedWith);

        private void OnTriggerExit2D(Collider2D other)
        {
            SolveExit(other.gameObject);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            SolveExit(other.gameObject);
        }

        protected virtual void SolveExit(GameObject exitWith) { }
    }
}