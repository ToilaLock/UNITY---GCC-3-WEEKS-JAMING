
public class BossIdleState : IState
{
    private BossStateMachine stateMachine;
    private BossController boss;

    public BossIdleState(BossStateMachine stateMachine, BossController boss)
    {
        this.stateMachine = stateMachine;
        this.boss = boss;
    }

    public void Enter()
    {
        boss.StopMovement();
        boss.bossAnimator.PlayIdle();
    }

    public void Exit() {}

    public void Update()
    {
        if (boss.IsPlayerInAttackRange() && boss.CanAttack())
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }

        if (boss.IsPlayerInDetectRange() && !boss.IsPlayerInAttackRange())
        {
            stateMachine.ChangeState(stateMachine.WalkState);
        }
    }

    public void FixedUpdate() { }
}