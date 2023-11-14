using System.Collections;
using Cinemachine;
using DG.Tweening;
using NaughtyAttributes;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Utils;

namespace Core
{
    public class PlayerControl : WeakSingleton<PlayerControl>
    {
        [Header("Components")]
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private Rigidbody2D playerRigidBody2D;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private SkeletonAnimation playerSkeletonAnimator;
        [SerializeField] private CinemachineImpulseSource impulseSource;
    
        [Header("Data")]
        [SerializeField] private float defaultSpeed;
        [SerializeField, Range(2, 20)] private int scaleFactor = 2;
        [SerializeField] private CurveHelper scaleCurve;
        [SerializeField] private CurveHelper speedCurve;
        [SerializeField] private float dashForce;
        [SerializeField] private float dashCoolDown;
        [SerializeField] private SpriteRenderer dashCoolDownImage;

        [Header("Game")] 
        [SerializeField, ReadOnly] private int lives;
        [SerializeField] private int score;
        [SerializeField] private float distance;

        //Internal Variables
        private Vector2 move = new(0,0);
        private float speedModifier = 1;
        private bool alive = true;
        private bool dashLeft;
        private bool dashRight;
        private bool isDashOnCooldown;

        private float scaleStep;
        [SerializeField, ReadOnly]
        private float linearScale;
        private bool scaleCooldown;

        //Actions
        private UnityAction<int> ScoreUpdateEvent;
        private UnityAction<int> LivesUpdateEvent;
        private UnityAction<float> DistanceUpdateEvent;
        private UnityAction<float> LinearScaleEvent;
        private UnityAction<int, float> GameOverEvent;
        
        //Cached
        private static readonly int SpeedModifier = Animator.StringToHash("SpeedModifier");
        private static readonly int Hit = Animator.StringToHash("GetHit");

        #region Input Actions
        public void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        }

        public void OnDashLeft(InputValue value)
        {
            if (isDashOnCooldown) return;
            dashLeft = true;
        }

        public void OnDashRight(InputValue value)
        {
            if (isDashOnCooldown) return;
            dashRight = true;
        }
        #endregion

        private void Awake()
        {
            ControlSingleton();
        
            AssessUtils.CheckRequirement(ref playerInput, this);
            AssessUtils.CheckRequirement(ref playerRigidBody2D, this);
            AssessUtils.CheckRequirement(ref playerAnimator, this);
            AssessUtils.CheckRequirement(ref playerSkeletonAnimator, this);
            AssessUtils.CheckRequirement(ref impulseSource, this);
        }

        private void Start()
        {
            scaleStep = 1.0f / scaleFactor;
            linearScale = 0.5f;

            score = 0;
            distance = 0.0f;
            UpdateScore(0);
            //Initialize lives with 3
            UpdateLives(3);
            UpdateDistance(0.0f);
            InitializeCurves();
            UpdateLinearValue();
        }

        private void Update()
        {
            if (!alive) return;
            Scale();
        }

        private void FixedUpdate()
        {
            if (!alive) return;
            Movement();
        }

        private void Movement()
        {
            var movement = Vector2.zero;
            if (dashLeft || dashRight)
            {
                Vector2 movementVector = new(dashForce * (dashLeft ? -1 : 1), 1.0f); //1.0 on y for always moving up
                movement = movementVector * Time.deltaTime;
                StartCoroutine(DashCooldownCoroutine());
            }
            else
            {
                Vector2 movementVector = new(move.x, 1.0f); //1.0 on y for always moving up
                movement = movementVector.normalized * (defaultSpeed * speedModifier * Time.deltaTime);
            }
            playerRigidBody2D.MovePosition(playerRigidBody2D.position + movement);

            UpdateDistance(1.0f * speedModifier * Time.deltaTime); //1.0 on y for always moving up
        }

        private void Scale()
        {
            if (scaleCooldown) return;
            var changed = false;
            switch (move.y)
            {
                case >= 0.5f:
                    linearScale += scaleStep;
                    changed = true;
                    break;
                case <= -0.5f:
                    linearScale -= scaleStep;
                    changed = true;
                    break;
            }

            linearScale = Mathf.Clamp(linearScale, 0.0f, 1.0f);
            if (!changed) return;
        
            //Executes the scaling tween
            scaleCooldown = true;
            var evaluatedScale = scaleCurve.Evaluate(linearScale);
            var scaleTo = new Vector3(evaluatedScale, evaluatedScale, 1.0f);
            transform.DOScale(scaleTo, 0.5f).OnComplete(() =>
            {
                scaleCooldown = false;
                transform.localScale = scaleTo;
                UpdateLinearValue();
                UpdateLinearSpeedModifier();
            });
        }

        public bool GetHit(SizeSO hitByObjectOfSize)
        {
            if (!hitByObjectOfSize.CanDamage(linearScale)) return false;
            
            impulseSource.GenerateImpulseWithForce(2.0f);
            UpdateLives(-1);
            playerAnimator.SetTrigger(Hit);

            if (lives > 0) return true;
            //Game Over
            GameOver();
            return true;
        }

        private IEnumerator DashCooldownCoroutine()
        {
            dashLeft = false;
            dashRight = false;
            isDashOnCooldown = true;
            dashCoolDownImage.color = Color.black;
            var tween = dashCoolDownImage.DOColor(Color.white, dashCoolDown);
            yield return new WaitForSeconds(dashCoolDown);
            isDashOnCooldown = false;
            tween.Kill();
            dashCoolDownImage.color = Color.white;
        }

        private void GameOver()
        {
            alive = false;
            DOTween.To(() => playerSkeletonAnimator.timeScale,
                value => playerSkeletonAnimator.timeScale = value,
                0.0f, 0.5f);
            GameOverEvent?.Invoke(score, distance);
        }

        private void UpdateLinearValue()
        {
            LinearScaleEvent?.Invoke(linearScale);

            switch (linearScale)
            {
                case <= 0.4f: playerSkeletonAnimator.skeleton.SetSkin("Turtle-Small");
                    break;
                case >= 0.6f: playerSkeletonAnimator.skeleton.SetSkin("Turtle-Normal");
                    break;
                default:playerSkeletonAnimator.skeleton.SetSkin("Turtle-Normal");
                    break;
            }
            
            playerSkeletonAnimator.Skeleton.SetSlotsToSetupPose();
            playerSkeletonAnimator.LateUpdate();
        }

        private void UpdateLinearSpeedModifier()
        {
            speedModifier = speedCurve.Evaluate(linearScale); 
            playerSkeletonAnimator.timeScale = speedModifier;
        }
      
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(move.x, move.y, 0));
        }

        [Button("Initialize Curves")]
        private void InitializeCurves()
        {
            scaleCurve.InitializeCurveHelper();
            speedCurve.InitializeCurveHelper();
        }
        
        #region Unity Actions Related

        public void UpdateLives(int incrementLife)
        {
            lives += incrementLife;
            LivesUpdateEvent?.Invoke(lives);
        }
        
        public void UpdateScore(int incrementScore)
        {
            score += incrementScore;
            ScoreUpdateEvent?.Invoke(score);
        }

        private void UpdateDistance(float incrementDistance)
        {
            distance += incrementDistance;
            DistanceUpdateEvent?.Invoke(distance);
        }

        public void RegisterUpdateScore(UnityAction<int> updateScoreAction)
        {
            ScoreUpdateEvent += updateScoreAction;
        }
    
        public void UnregisterUpdateScore(UnityAction<int> updateScoreAction)
        {
            ScoreUpdateEvent -= updateScoreAction;
        }
    
        public void RegisterUpdateLives(UnityAction<int> updateLiveAction)
        {
            LivesUpdateEvent += updateLiveAction;
        }
    
        public void UnregisterUpdateLives(UnityAction<int> updateLiveAction)
        {
            LivesUpdateEvent -= updateLiveAction;
        }
        
        public void RegisterUpdateDistance(UnityAction<float> updateDistanceAction)
        {
            DistanceUpdateEvent += updateDistanceAction;
        }
    
        public void UnregisterUpdateDistance(UnityAction<float> updateDistanceAction)
        {
            DistanceUpdateEvent -= updateDistanceAction;
        }

        public void RegisterUpdateLinearValues(UnityAction<float> updateLinearValueAction)
        {
            LinearScaleEvent += updateLinearValueAction;
        }
    
        public void UnregisterUpdateLinearValues(UnityAction<float> updateLinearValueAction)
        {
            LinearScaleEvent -= updateLinearValueAction;
        }
        
        public void RegisterGameOverEvent(UnityAction<int, float> gameOverEvent)
        {
            GameOverEvent += gameOverEvent;
        }
    
        public void UnregisterGameOverEvent(UnityAction<int, float> gameOverEvent)
        {
            GameOverEvent -= gameOverEvent;
        }
        #endregion
    }
}
