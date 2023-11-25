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
        [SerializeField] private SkeletonAnimation playerSkeletonAnimator;
        [SerializeField] private CinemachineImpulseSource impulseSource;
        [SerializeField] private Transform mouthPlacement;
    
        [Header("Data")]
        [SerializeField] private float defaultSpeed;
        [SerializeField, Range(2, 20)] private int scaleFactor = 2;
        [SerializeField] private CurveHelper scaleCurve;
        [SerializeField] private CurveHelper speedCurve;
        [SerializeField] private float dashDefaultTimer = 10.0f;
        [SerializeField] private float invincibleDefaultTimer = 10.0f;
        [SerializeField] private float doubleDefaultTimer = 10.0f;
        [SerializeField] private float dashForce;
        [SerializeField] private float dashCoolDown;
        [SerializeField] private SpriteRenderer dashCoolDownImage;

        [Header("References")] 
        [SerializeField] private GameObject dashEffectGameObject;
        [SerializeField] private GameObject invincibleEffectGameObject;
        [SerializeField] private GameObject doublePointsEffectGameObject;

        [Header("Game")] 
        [SerializeField, ReadOnly] private int lives;
        [SerializeField] private int score;
        [SerializeField] private float distance;
        [SerializeField, ReadOnly] private float linearScale;

        //Internal Variables
        private Vector2 move = new(0,0);
        private float speedModifier = 1;
        private bool alive = true;
        //Internal Dash Variables
        private bool dashActive;
        private float dashTimer;
        private bool dashLeft;
        private bool dashRight;
        private bool isDashOnCooldown;
        
        //Internal Invincible Variables
        private bool invisible;
        //Internal Double Variables
        private bool doublePoints;
        
        private float scaleStep;
        private bool scaleCooldown;
        
        private Coroutine powerupCoroutine;

        //Actions
        private UnityAction<int> ScoreUpdateEvent;
        private UnityAction<int> LivesUpdateEvent;
        private UnityAction<float> DistanceUpdateEvent;
        private UnityAction<float> LinearScaleEvent;
        private UnityAction<CollectableControl> GetCollectable;
        private UnityAction<int, float> GameOverEvent;

        #region Input Actions
        public void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        }

        public void OnDashLeft(InputValue value)
        {
            if (!dashActive) return;
            if (isDashOnCooldown) return;
            dashLeft = true;
        }

        public void OnDashRight(InputValue value)
        {
            if (!dashActive) return;
            if (isDashOnCooldown) return;
            dashRight = true;
        }
        #endregion

        private void Awake()
        {
            ControlSingleton();
        
            AssessUtils.CheckRequirement(ref playerInput, this);
            AssessUtils.CheckRequirement(ref playerRigidBody2D, this);
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
                switch (move.x)
                {
                    case < -0.2f:
                        if (playerSkeletonAnimator.AnimationName != "Left")
                        {
                            playerSkeletonAnimator.state.SetAnimation(0, "Left", true);    
                        }
                        break;
                    case > 0.2f:
                        if (playerSkeletonAnimator.AnimationName != "Right")
                        {
                            playerSkeletonAnimator.state.SetAnimation(0, "Right", true);
                        }
                        break;
                    default:
                        if (playerSkeletonAnimator.AnimationName != "Forward")
                        {
                            playerSkeletonAnimator.state.SetAnimation(0, "Forward", true);
                        }
                        break;
                }
                 
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
            if (invisible) return true;
            if (!hitByObjectOfSize.CanDamage(linearScale)) return false;
            
            impulseSource.GenerateImpulseWithForce(2.0f);
            UpdateLives(-1);
            // playerAnimator.SetTrigger(Hit);

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

            if (mouthPlacement != null)
            {
                Gizmos.DrawWireSphere(mouthPlacement.position, 2.0f);
            }
        }

        
        public void ActivateDash()
        {
            ActivatePowerUp(ref dashActive, dashEffectGameObject, dashDefaultTimer);
        }

        public void ActivateInvincibility()
        {
            ActivatePowerUp(ref invisible, invincibleEffectGameObject, invincibleDefaultTimer);
        }

        public void ActivateDoublePoints()
        {
            ActivatePowerUp(ref doublePoints, doublePointsEffectGameObject, doubleDefaultTimer);
        }

        private void ActivatePowerUp(ref bool powerUp, GameObject effectGameObject, float timer)
        {
            if (powerUp)
            {
                if (powerupCoroutine != null)
                {
                    StopCoroutine(powerupCoroutine);
                }
            }

            DeactivatePowerUps();
            powerUp = true;
            powerupCoroutine = StartCoroutine(PowerUpCoroutine(effectGameObject, timer));
        }

        private void DeactivatePowerUps()
        {
            dashActive = false;
            invisible = false;
            doublePoints = false;
        }

        private IEnumerator PowerUpCoroutine(GameObject effectGameObject, float timer)
        {
            effectGameObject?.SetActive(true);
            yield return new WaitForSeconds(timer);
            DeactivatePowerUps();
            effectGameObject?.SetActive(false);
        }
        
        public Vector3 GetMouthPlacement() => mouthPlacement.position;

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

        public void Collect(CollectableControl collectable)
        {
            GetCollectable?.Invoke(collectable);
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
        
        public void RegisterGetCollectableEvent(UnityAction<CollectableControl> getCollectableEvent)
        {
            GetCollectable += getCollectableEvent;
        }
    
        public void UnregisterGetCollectableEvent(UnityAction<CollectableControl> getCollectableEvent)
        {
            GetCollectable -= getCollectableEvent;
        }
        #endregion
    }
}
