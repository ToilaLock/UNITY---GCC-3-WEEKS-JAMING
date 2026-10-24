using UnityEngine;

public class PlayerDeadState : IState
{
    private PlayerController playerController;

    public PlayerDeadState(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void Enter()
    {
        playerController.StopMovement();
        playerController.DisableHitBox();

        playerController.playerAnimator.PlayDead();

        playerController.rb.linearVelocity = Vector2.zero;
    }

    public void Exit() { }
    public void Update() { }
    public void FixedUpdate() { }
}