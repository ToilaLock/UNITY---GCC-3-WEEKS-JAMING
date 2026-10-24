using System;
using UnityEngine;
public class PlayerRollState : IState
{
    private PlayerStateMachine stateMachine;
    private PlayerInputHandler inputHandler;
    private PlayerController playerController;

    private float rollDuration;
    
    public PlayerRollState(
        PlayerStateMachine stateMachine,
        PlayerInputHandler inputHandler,
        PlayerController playerController)
    {
        this.stateMachine = stateMachine;
        this.inputHandler = inputHandler;
        this.playerController = playerController;
    }

    public bool CanRoll()
    {
        return playerController.isGrounded && playerController.HasStamina();
    }

    public void Enter()
    {
        Debug.Log("Enter Roll State");
        rollDuration = playerController.RollDuration;

        if (playerController.HealthComponent != null)
            playerController.HealthComponent.IsInvincible = true;

        playerController.UseStamina(playerController.RollStamina);

        if (playerController.HealthComponent.CurrentHealth <= 0) return;

        playerController.SetInvincibleVisual(true);

        playerController.playerAnimator.PlayRoll();
        playerController.Roll();
    }

    public void Exit()
    {
        playerController.SetInvincibleVisual(false);
        playerController.HealthComponent.IsInvincible = false;
            
        Debug.Log("Exit Roll State");
    }

    public void Update()
    {
        rollDuration -= Time.deltaTime;

        if (!playerController.isGrounded)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
        
        if(rollDuration <= 0)
        {
            if (Math.Abs(inputHandler.MoveInput.x) > 0)
                stateMachine.ChangeState(stateMachine.MoveState);
            else
                stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public void FixedUpdate()
    {
        
    }
}
