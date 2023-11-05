using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
using Utils;

namespace Core
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BackgroundSpawner : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private List<Sprite> backgrounds;
        [SerializeField]
        private Transform spawnNextPoint;
        [SerializeField]
        private List<BackgroundSpawner> backgroundPrefabs;
        [SerializeField] 
        private List<SpawnAreaControl> spawners;
        
        [SerializeField] 
        private float killTimer;

        private void Awake()
        {
            AssessUtils.CheckRequirement(ref spriteRenderer, this);
        }

        private void OnEnable()
        {
            SetRandomBackgroundSprite();
        }

        private void OnDisable()
        {
            spawners.ForEach(spawner =>
            {
                spawner.gameObject.SetActive(false);
            });
        }

        private void Start()
        {
            SetRandomBackgroundSprite();
        }

        private void SetRandomBackgroundSprite()
        {
            spriteRenderer.sprite = RandomHelper<Sprite>.GetRandomFromList(backgrounds);
        }

        public void SpawnNextBackground()
        {
            var prefab = RandomHelper<BackgroundSpawner>.GetRandomFromList(backgroundPrefabs);
            LeanPool.Spawn(prefab, spawnNextPoint.position, Quaternion.identity);
        }

        public void StartKillTimer()
        {
            StartCoroutine(KillCoroutine());
        }

        private IEnumerator KillCoroutine()
        {
            yield return new WaitForSeconds(killTimer);
            LeanPool.Despawn(gameObject, 0.1f);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(spawnNextPoint.position, 1.0f);
        }
    }
}
