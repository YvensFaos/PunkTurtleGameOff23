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

        // ReSharper disable once MemberCanBePrivate.Global
        public static bool ValidPosition(Vector2 position, Vector2 size, ref RaycastHit2D[] colliders)
        {
            return Physics2D.BoxCastNonAlloc(position, size, 0f, new Vector2(1.0f,0.0f), colliders, 0.01f) == 0;
        }

        public static bool ValidPosition(Vector2 position, Bounds bounds, ref RaycastHit2D[] colliders)
        {
            var size = new Vector2(bounds.size.x, bounds.size.y);
            return ValidPosition(position, size, ref colliders);
        }
    }
}