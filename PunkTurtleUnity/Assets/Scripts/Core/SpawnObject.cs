using NaughtyAttributes;
using UnityEngine;

namespace Core
{
    public class SpawnObject : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private Collider2D spawnCollider2D;
        [SerializeField, EnableIf("HasCollider2D")]
        private bool checkPlacement = true;

        public SpriteRenderer SpawnSpriteRenderer => spriteRenderer;
        public Collider2D SpawnCollider2D => spawnCollider2D;
        public bool HasCollider =>  HasCollider2D && checkPlacement;
        private bool HasCollider2D => spawnCollider2D != null;
    }
}