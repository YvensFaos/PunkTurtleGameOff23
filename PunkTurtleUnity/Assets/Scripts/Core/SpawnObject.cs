using UnityEngine;

namespace Core
{
    public class SpawnObject : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private Collider2D spawnCollider2D;

        public SpriteRenderer SpawnSpriteRenderer => spriteRenderer;
        public Collider2D SpawnCollider2D => spawnCollider2D;
        public bool HasCollider => spawnCollider2D != null;
    }
}