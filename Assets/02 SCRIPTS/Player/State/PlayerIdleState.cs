using UnityEngine;

public class PlayerIdleState : IState
{
    private PlayerInputHandler inputHandler;
    private PlayerStateMachine playerStateMachine;
    private PlayerController playerController;

    private PlayerMoveState moveState;
    private PlayerJumpState jumpState;

    public PlayerIdleState
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
        Debug.Log("Enter Idle State");
    }

    public void Exit()
    {
        Debug.Log("Exit Idle State");
    }

    public void Update()
    {
        if (inputHandler.MoveInput.x != 0)
        {
            playerStateMachine.ChangeState(moveState);
        }
    }

    public void FixedUpdate()
    {
        
    }

    public void SetMoveState(PlayerMoveState moveState)
    {
        this.moveState = moveState;
    }

    public void SetJumpState(PlayerJumpState jumpState)
    {
        this.jumpState = jumpState;
    }
}