using UnityEngine;

public class PlayerIdleState : IState
{
    private PlayerStateMachine stateMachine;
    private PlayerInputHandler inputHandler;
    private PlayerController playerController;

    public PlayerIdleState(
        PlayerStateMachine stateMachine, 
        PlayerInputHandler inputHandler, 
        PlayerController playerController)
    {
        this.stateMachine = stateMachine;
        this.inputHandler = inputHandler;
        this.playerController = playerController;
    }

    public void Enter()
    {
        playerController.StopMovement(); 
        playerController.playerAnimator.PlayIdle(); 
    }

    public void Exit() { }

    public void Update()
    {
        if (!playerController.isGrounded && playerController.verticalVelocity <= 0)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }

        if (inputHandler.MoveInput.x != 0)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
        }
    }

    public void FixedUpdate() { }
}