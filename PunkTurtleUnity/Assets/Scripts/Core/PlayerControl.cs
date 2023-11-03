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

    //Internal Variables
    private Vector2 move = new(0,0);
    private float speedModifier = 1;
    
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
    { }

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(move.x, move.y, 0));
    }
}
