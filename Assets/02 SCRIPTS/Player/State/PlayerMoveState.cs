using UnityEngine;

public class PlayerMoveState : IState
{
    private PlayerInputHandler inputHandler;
    private PlayerStateMachine playerStateMachine;
    private PlayerController playerController;

    private PlayerIdleState idleState;
    private PlayerJumpState jumpState;

    public PlayerMoveState
    (PlayerStateMachine playerStateMachine,
    PlayerInputHandler inputHandler,
    PlayerController playerController)
    {
        this.playerStateMachine = playerStateMachine;
        this.inputHandler = inputHandler;
        this.playerController = playerController;
    }

    public void Enter()
    {
        Debug.Log("Enter Move State");
    }

    public void Exit()
    {
        Debug.Log("Exit Move State");
    }

    public void Update()
    {
        Debug.Log($"MoveState Input: {inputHandler.MoveInput.x}");

        if (inputHandler.MoveInput.x == 0)
        {
            playerController.StopMovement();
            playerStateMachine.ChangeState(idleState);
        }
    }

    public void FixedUpdate()
    {
        playerController.Move();
        playerController.Flip();
    }

    public void SetIdleState(PlayerIdleState idleState)
    {
        this.idleState = idleState;
    }

    public void SetJumpState(PlayerJumpState jumpState)
    {
        this.jumpState = jumpState;
    }
}