using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

public class PlayerControl : WeakSingleton<PlayerControl>
{
    [Header("Components")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody2D playerRigidBody2D;
    
    [Header("Data")]
    [SerializeField] private float defaultSpeed;
    [SerializeField, Range(0.01f, 2.0f)] private float scaleFactor = 0.125f;

    //Internal Variables
    private Vector2 move = new(0,0);
    private float speedModifier = 1;
    private float accScale = 0;
    private bool scaleCooldown = false;
    
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
    }

    private void Scale()
    {
        if (scaleCooldown) return;
        var changed = false;
        switch (move.y)
        {
            case >= 0.5f:
                accScale += scaleFactor;
                changed = true;
                break;
            case <= -0.5f:
                accScale -= scaleFactor;
                changed = true;
                break;
        }

        accScale = Mathf.Clamp(accScale, scaleFactor - 1.0f, 2.0f - scaleFactor);
        if (!changed) return;
        
        //Executes the scaling tween
        scaleCooldown = true;
        transform.DOScale(new Vector3(1.0f + accScale, 1.0f + accScale, 1.0f), 0.5f).OnComplete(() =>
        {
            scaleCooldown = false;
            transform.localScale = new Vector3(1.0f + accScale, 1.0f + accScale, 1.0f);
        });
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(move.x, move.y, 0));
    }
}
