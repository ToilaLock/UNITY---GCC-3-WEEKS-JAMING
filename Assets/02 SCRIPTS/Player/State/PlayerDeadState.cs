using UnityEngine;

public class PlayerDeadState : IState
{
    private PlayerController playerController;
    private PlayerAudio playerAudio;

    public PlayerDeadState(PlayerController playerController,
                            PlayerAudio playerAudio)
    {
        this.playerController = playerController;
        this.playerAudio = playerAudio;
    }

    public void Enter()
    {
        playerController.StopMovement();
        playerController.DisableHitBox();
        playerAudio.PlayDeadSound();
        playerController.playerAnimator.PlayDead();

        playerController.rb.linearVelocity = Vector2.zero;
    }

    public void Exit() { }
    public void Update() { }
    public void FixedUpdate() { }
}