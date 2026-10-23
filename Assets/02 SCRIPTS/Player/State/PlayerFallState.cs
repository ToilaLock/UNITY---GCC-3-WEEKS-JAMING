using UnityEngine;

public class PlayerFallState : IState
{
    private PlayerStateMachine stateMachine;
    private PlayerInputHandler inputHandler;
    private PlayerController playerController;
    private float coyoTimeCounter;

    public PlayerFallState(
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
        coyoTimeCounter = playerController.CoyoteTime;
        playerController.playerAnimator.PlayFall(); 
    }

    public void Exit() { }

    public void Update()
    {
        coyoTimeCounter -= Time.deltaTime;

        if (!playerController.isGrounded) return;

        if (inputHandler.JumpHold)
        {
            stateMachine.ChangeState(stateMachine.JumpState);
        }
        else if (inputHandler.MoveInput.x != 0)
            stateMachine.ChangeState(stateMachine.MoveState);
        else
            stateMachine.ChangeState(stateMachine.IdleState);
    }

    public void FixedUpdate()
    {
        playerController.Move();
        playerController.Flip();
    }

    public bool CanCoyoteJump() => coyoTimeCounter > 0f;
}