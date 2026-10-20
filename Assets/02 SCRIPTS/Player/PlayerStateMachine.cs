using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    private PlayerInputHandler inputHandler;
    private StateMachine statemachine;
    private PlayerController playerController;

    private PlayerIdleState idleState;
    private PlayerMoveState moveState;
    private PlayerJumpState jumpState;

    private void Awake()
    {
        statemachine = new StateMachine();
        
        inputHandler = GetComponent<PlayerInputHandler>();
        playerController = GetComponent<PlayerController>();
        
        idleState = new PlayerIdleState(this, inputHandler, playerController);
        moveState = new PlayerMoveState(this, inputHandler, playerController);
        jumpState = new PlayerJumpState(this, inputHandler, playerController);
        idleState.SetMoveState(moveState);
        idleState.SetJumpState(jumpState);
        moveState.SetIdleState(idleState);
        moveState.SetJumpState(jumpState);
        jumpState.SetIdleState(idleState);
        jumpState.SetMoveState(moveState);

        inputHandler.JumpPressed += OnJumpPressed;

        statemachine.Initialize(idleState);
    }

    private void Update()
    {
        statemachine.Update();
    }

    private void FixedUpdate()
    {
        statemachine.FixedUpdate();
    }

    public void ChangeState(IState newState)
    {
        statemachine.ChangeState(newState);
    }

    private void OnJumpPressed()
    {
        statemachine.ChangeState(jumpState);
    }
}