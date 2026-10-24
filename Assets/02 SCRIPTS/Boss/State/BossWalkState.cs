
public class BossWalkState : IState
{
    private BossStateMachine stateMachine;
    private BossController boss;

    public BossWalkState(BossStateMachine stateMachine, BossController boss)
    {
        this.stateMachine = stateMachine;
        this.boss = boss;
    }

    public void Enter()
    {
        boss.bossAnimator.PlayWalk();
    }

    public void Exit()
    {
        boss.StopMovement();
    }

    public void Update()
    {
        if (boss.IsPlayerInAttackRange())
        {
            if (boss.CanAttack())
                stateMachine.ChangeState(stateMachine.AttackState);
            else
                stateMachine.ChangeState(stateMachine.IdleState);

            return;
        }

        if (!boss.IsPlayerInDetectRange())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public void FixedUpdate()
    {
        boss.MoveTowardsPlayer();
    }
}