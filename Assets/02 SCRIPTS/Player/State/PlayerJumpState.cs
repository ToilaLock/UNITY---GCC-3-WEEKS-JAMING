using UnityEngine;

public class PlayerJumpState : IState
{
    private PlayerStateMachine stateMachine;
    private PlayerInputHandler inputHandler;
    private PlayerController playerController;
    private bool hasJumped;

    public PlayerJumpState(
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
        playerController.Jump();
        playerController.playerAnimator.PlayJump(); 
        hasJumped = false;
    }

    public void Exit() { }

    public void Update()
    {
        if (!hasJumped && !playerController.isGrounded)
        {
            hasJumped = true;
            return;
        }

        if (hasJumped && playerController.isFalling)
        {
            stateMachine.ChangeState(stateMachine.FallState);
        }
    }

    public void FixedUpdate()
    {
        playerController.Move();
        playerController.Flip();
    }
}