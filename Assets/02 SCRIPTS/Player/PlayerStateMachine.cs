using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private PlayerInputHandler inputHandler;
    private StateMachine stateMachine;
    private PlayerController playerController;


    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }

    public PlayerAttackState AttackState { get; private set; }
    public PlayerRollState RollState { get; private set; }


    private void Awake()
    {
        stateMachine = new StateMachine();
        inputHandler = GetComponent<PlayerInputHandler>();
        playerController = GetComponent<PlayerController>();
        
        IdleState = new PlayerIdleState(this, inputHandler, playerController);
        MoveState = new PlayerMoveState(this, inputHandler, playerController);
        JumpState = new PlayerJumpState(this, inputHandler, playerController);
        FallState = new PlayerFallState(this, inputHandler, playerController);

        AttackState = new PlayerAttackState(this, inputHandler, playerController);
        RollState = new PlayerRollState(this, inputHandler, playerController);
    }

    private void Start() => stateMachine.Initialize(IdleState);
    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();
    public void ChangeState(IState newState) => stateMachine.ChangeState(newState);

    private void OnEnable() 
    {
        inputHandler.JumpPressed += OnJumpPressed;
        inputHandler.RollPressed += OnRollPressed;
    }
    private void OnDisable()
    {
        inputHandler.JumpPressed -= OnJumpPressed;
        inputHandler.RollPressed -= OnRollPressed;
    }

    private void OnJumpPressed()
    {
        if (playerController.isGrounded)
        {
            stateMachine.ChangeState(JumpState);
            return;
        }

        if (stateMachine.CurrentState == FallState && FallState.CanCoyoteJump())
        {
            stateMachine.ChangeState(JumpState);
        }
    }

    private void OnRollPressed()
    {
        if (stateMachine.CurrentState == RollState) 
            return;
        if (RollState.CanRoll())
        {
            stateMachine.ChangeState(RollState);
        }
    }

    private void OnAttackPressed()
    {
        if (stateMachine.CurrentState == RollState ||
            stateMachine.CurrentState == JumpState ||
            !playerController.isGrounded)
            return;

        stateMachine.ChangeState(AttackState);
    }
}