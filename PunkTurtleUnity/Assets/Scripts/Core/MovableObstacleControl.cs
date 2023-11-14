using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Utils;

namespace Core
{
    public class MovableObstacleControl : ObstacleControl
    {
       
        [SerializeField]
        private Vector2 movementDirection;
        [SerializeField] 
        private float movementSpeed;
        [SerializeField]
        private bool randomDirection;
        [SerializeField, ShowIf("randomDirection")] 
        private float randomMovement;
        [SerializeField, ShowIf("randomDirection")]
        private float randomTimer;
        [SerializeField, ShowIf("randomDirection")] 
        private SpriteRenderer sprite;
        [SerializeField, ShowIf("randomDirection")] 
        private bool mirrorSprite;
        [SerializeField] 
        private float distanceKill;
        [SerializeField, Unity.Collections.ReadOnly]
        private float currentDistance;
        
        private Tweener scalingTweener;
        private bool randomMoving;
        private bool shouldDie;
        protected void OnEnable()
        {
            shouldDie = false;
            randomMoving = false;
            
            scalingTweener?.Kill();
            scalingTweener = transform.DOPunchScale(new Vector3(0.525f, 0.525f, 1.0f), 0.3f, 1, 0.1F).SetLoops(-1);
            
            StopAllCoroutines();
            if (randomDirection)
            {
                StartCoroutine(MovementCoroutine());
            }
        }

        protected void OnDisable()
        {
            scalingTweener?.Kill();
        }

        private void Update()
        {
            if (randomDirection) return;
            transform.Translate(movementDirection * movementSpeed);
            CheckKillDistance();
        }

        private void LateUpdate()
        {
            if (!shouldDie) return;
            Destroy(gameObject);
        }

        private IEnumerator MovementCoroutine()
        {
            while (!IsFarAway())
            {
                movementDirection = RandomPointUtils.GenerateRandomDirection2D() * randomMovement;

                if (mirrorSprite && sprite != null)
                {
                    sprite.flipX = movementDirection.x > 0;
                    sprite.flipY = movementDirection.y > 0;
                }
                
                var objective = transform.position + new Vector3(movementDirection.y, movementDirection.y, 0);
                randomMoving = false;
                transform.DOMove(objective, randomTimer).OnComplete(() =>
                {
                    randomMoving = true;
                });
                yield return new WaitUntil(() => randomMoving);

                //30% chance of not moving for half of the movement timer
                if (RandomChanceUtils.GetChance(30.0f))
                {
                    yield return new WaitForSeconds(randomTimer / 2.0f);
                }
            }

            shouldDie = true;
        }

        private void CheckKillDistance()
        {
            if (IsFarAway())
            {
                shouldDie = true;
            }
        }

        private bool IsFarAway()
        {
            var distanceVector = PlayerControl.GetSingleton().transform.position - transform.position;
            currentDistance = distanceVector.magnitude;
            return distanceVector.sqrMagnitude > (distanceKill * distanceKill);
        }
    }
}