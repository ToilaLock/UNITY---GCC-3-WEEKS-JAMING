using UnityEngine;

public class PlayerHurtState : IState
{
    private PlayerStateMachine stateMachine;
    private PlayerController playerController;

    private float hurtDuration = 0.3f; 
    private float timer;

    public PlayerHurtState(PlayerStateMachine stateMachine, PlayerController playerController)
    {
        this.stateMachine = stateMachine;
        this.playerController = playerController;
    }

    public void Enter()
    {
        timer = hurtDuration;
        playerController.DisableHitBox(); 
        playerController.playerAnimator.PlayHurt();
    }

    public void Exit()
    {
        playerController.StopMovement();
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            if (!playerController.isGrounded)
                stateMachine.ChangeState(stateMachine.FallState);
            else
                stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public void FixedUpdate() { }
}