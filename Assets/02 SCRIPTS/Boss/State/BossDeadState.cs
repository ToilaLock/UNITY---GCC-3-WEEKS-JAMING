using UnityEngine;

public class BossDeadState : IState
{
    private BossController boss;

    public BossDeadState(BossController boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {

        boss.StopMovement();
        boss.DisableHitBox();
        boss.bossAnimator.PlayDead();

        boss.gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void Exit() { }
    public void Update() { }
    public void FixedUpdate() { }
}