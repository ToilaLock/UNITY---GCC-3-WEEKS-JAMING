using UnityEngine;

public class PlayerJumpState : IState
{
    private PlayerInputHandler inputHandler;
    private PlayerStateMachine playerStateMachine;
    private PlayerController playerController;

    private PlayerIdleState idleState;
    private PlayerMoveState moveState;

    private bool hasJumped = false;

    public PlayerJumpState(
        PlayerStateMachine playerStateMachine,
        PlayerInputHandler inputHandler,
        PlayerController playerController)
    {
        this.playerStateMachine = playerStateMachine;
        this.inputHandler = inputHandler;
        this.playerController = playerController;
    }

    public void Enter()
    {
        Debug.Log("Enter Jump State");
        hasJumped = false;
        playerController.Jump();
    }

    public void Exit()
    {
        Debug.Log("Exit Jump State");
    }

    public void Update()
    {
        if (!hasJumped)
        {
            if (!playerController.isGrounded)
            {
                hasJumped = true;
            }

            return;
        }

        if (playerController.isGrounded)
        {
            if (inputHandler.MoveInput.x != 0)
            {
                playerStateMachine.ChangeState(moveState);
            }
            else
            {
                playerStateMachine.ChangeState(idleState);
            }
        }
    }

    public void FixedUpdate()
    {
        playerController.Jump();
    }

    public void SetIdleState(PlayerIdleState idleState)
    {
        this.idleState = idleState;
    }

    public void SetMoveState(PlayerMoveState moveState)
    {
        this.moveState = moveState;
    }
}