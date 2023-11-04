using DG.Tweening;
using NaughtyAttributes;
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
    
        [Header("Data")]
        [SerializeField] private float defaultSpeed;
        [SerializeField, Range(2, 20)] private int scaleFactor = 2;
        [SerializeField] private int score;
        [SerializeField] private float distance;
        [SerializeField] private CurveHelper scaleCurve;
        [SerializeField] private CurveHelper speedCurve;

        //Internal Variables
        private Vector2 move = new(0,0);
        private float speedModifier = 1;

        private float scaleStep;
        [SerializeField, ReadOnly]
        private float linearScale;
        private bool scaleCooldown;

        //Actions
        private UnityAction<int> ScoreUpdateEvent;
        private UnityAction<float> DistanceUpdateEvent;
        private UnityAction<float> LinearScaleEvent;
        
        //Cached
        private static readonly int SpeedModifier = Animator.StringToHash("SpeedModifier");

        #region Input Actions
        public void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        }
        #endregion

        private void Awake()
        {
            ControlSingleton();
        
            AssessUtils.CheckRequirement(ref playerInput, this);
            AssessUtils.CheckRequirement(ref playerRigidBody2D, this);
            AssessUtils.CheckRequirement(ref playerAnimator, this);
        }

        private void Start()
        {
            //Steps 
            scaleStep = 1.0f / scaleFactor;
            linearScale = 0.5f;
            
            score = 0;
            distance = 0.0f;
            UpdateScore(0);
            UpdateDistance(0.0f);
            InitializeCurves();
            UpdateLinearValue();
        }

        private void Update()
        {
            Scale();
        }

        private void FixedUpdate()
        {
            Movement();   
        }

        private void Movement()
        {
            //Always move up
            Vector2 movementVector = new(move.x, 1.0f);
            var movement = movementVector.normalized * (defaultSpeed * speedModifier * Time.deltaTime);
            playerRigidBody2D.MovePosition(playerRigidBody2D.position + movement);
        
            UpdateDistance(1.0f * speedModifier * Time.deltaTime);
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

        private void UpdateLinearValue()
        {
            LinearScaleEvent?.Invoke(linearScale);
        }

        private void UpdateLinearSpeedModifier()
        {
            speedModifier = speedCurve.Evaluate(linearScale); 
            playerAnimator.SetFloat(SpeedModifier, speedModifier);
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
        #endregion
    }
}
