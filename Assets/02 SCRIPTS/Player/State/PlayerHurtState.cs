using UnityEngine;

public class PlayerHurtState : IState
{
    private PlayerStateMachine stateMachine;
    private PlayerController playerController;
    private PlayerAudio playerAudio;

    private float hurtDuration = 0.3f; 
    private float timer;

    public PlayerHurtState(PlayerStateMachine stateMachine, 
            PlayerController playerController,
            PlayerAudio playerAudio)
    {
        this.stateMachine = stateMachine;
        this.playerController = playerController;
        this.playerAudio = playerAudio;
    }

    public void Enter()
    {
        timer = hurtDuration;
        playerController.DisableHitBox(); 
        playerAudio.PlayHurtSound();
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