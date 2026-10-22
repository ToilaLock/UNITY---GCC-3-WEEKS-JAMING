using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private PlayerInputHandler inputHandler;
    private StateMachine statemachine;
    private PlayerController playerController;


    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }


    private void Awake()
    {
        statemachine = new StateMachine();
        inputHandler = GetComponent<PlayerInputHandler>();
        playerController = GetComponent<PlayerController>();
        
        IdleState = new PlayerIdleState(this, inputHandler, playerController);
        MoveState = new PlayerMoveState(this, inputHandler, playerController);
        JumpState = new PlayerJumpState(this, inputHandler, playerController);
        FallState = new PlayerFallState(this, inputHandler, playerController);
    }

    private void Start() => statemachine.Initialize(IdleState);
    private void Update() => statemachine.Update();
    private void FixedUpdate() => statemachine.FixedUpdate();
    public void ChangeState(IState newState) => statemachine.ChangeState(newState);

    private void OnEnable() => inputHandler.JumpPressed += OnJumpPressed;
    private void OnDisable() => inputHandler.JumpPressed -= OnJumpPressed;

    private void OnJumpPressed()
    {
        if (playerController.isGrounded)
        {
            statemachine.ChangeState(JumpState);
            return;
        }

        if (statemachine.CurrentState == FallState && FallState.CanCoyoteJump())
        {
            statemachine.ChangeState(JumpState);
        }
    }
}