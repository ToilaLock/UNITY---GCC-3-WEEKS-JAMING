using UnityEngine;

public class PlayerMoveState : IState
{
    private PlayerStateMachine stateMachine;
    private PlayerInputHandler inputHandler;
    private PlayerController playerController;
    private PlayerAudio playerAudio;

    public PlayerMoveState(
        PlayerStateMachine stateMachine, 
        PlayerInputHandler inputHandler, 
        PlayerController playerController,
        PlayerAudio playerAudio)
    {
        this.stateMachine = stateMachine;
        this.inputHandler = inputHandler;
        this.playerController = playerController;
        this.playerAudio = playerAudio;
    }

    public void Enter()
    {
        playerController.playerAnimator.PlayRun(); 
    }

    public void Exit() { }

    public void Update()
    {
        if (!playerController.isGrounded && playerController.verticalVelocity <= 0)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }

        if (inputHandler.MoveInput.x == 0)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public void FixedUpdate()
    {
        playerController.Move();
        playerController.Flip();
    }
}