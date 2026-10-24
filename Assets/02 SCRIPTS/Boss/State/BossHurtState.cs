using UnityEngine;

public class BossHurtState : IState
{
    private BossStateMachine stateMachine;
    private BossController boss;

    private float hurtDuration = 0.35f;
    private float timer;

    public BossHurtState(BossStateMachine stateMachine, BossController boss)
    {
        this.stateMachine = stateMachine;
        this.boss = boss;
    }

    public void Enter()
    {
        timer = hurtDuration;
        boss.DisableHitBox(); 
        boss.bossAnimator.PlayerTakeDamage();
    }

    public void Exit()
    {
        boss.StopMovement();
    }

    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public void FixedUpdate() { }
}