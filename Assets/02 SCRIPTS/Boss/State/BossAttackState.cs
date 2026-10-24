using UnityEngine;

public class BossAttackState : IState
{
    private BossStateMachine stateMachine;
    private BossController boss;

    public BossAttackState(BossStateMachine stateMachine, BossController boss)
    {
        this.stateMachine = stateMachine;
        this.boss = boss;
    }

    public void Enter()
    {
        boss.StopMovement();
        boss.DisableHitBox();
        boss.bossAnimator.PlayAttack();
    }

    public void Exit()
    {
        boss.DisableHitBox();
        boss.LastAttackTime = Time.time;
    }

    public void Update() { }
    public void FixedUpdate() { }

    public void OnAttackAnimationFinished()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }
}