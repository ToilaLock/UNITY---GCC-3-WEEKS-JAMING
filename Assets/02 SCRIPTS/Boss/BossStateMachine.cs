using UnityEngine;

public class BossStateMachine : MonoBehaviour
{
    private StateMachine stateMachine;
    private BossController bossController;
    private BossHealth bossHealth;

    public BossIdleState IdleState { get; private set; }
    public BossWalkState WalkState { get; private set; }
    public BossAttackState AttackState { get; private set; }
    public BossHurtState HurtState { get; private set; }
    public BossDeadState DeadState { get; private set; }

    public IState CurrentState => stateMachine.CurrentState;

    private void Awake()
    {
        bossController = GetComponent<BossController>();
        bossHealth = GetComponent<BossHealth>();

        stateMachine = new StateMachine();
        IdleState = new BossIdleState(this, bossController);
        WalkState = new BossWalkState(this, bossController);
        AttackState = new BossAttackState(this, bossController);

        HurtState = new BossHurtState(this, bossController);
        DeadState = new BossDeadState(bossController);
    }

    private void Start()
    {
        stateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    public void ChangeState(IState newState)
    {
        stateMachine.ChangeState(newState);
    }

    // DAMAGE AND DEAD
    private void OnEnable()
    {
        if (bossHealth == null)
            bossHealth = GetComponent<BossHealth>();

        if (bossHealth != null)
        {
            bossHealth.OnDamaged += HandleDamaged;
            bossHealth.OnDeath += HandleDeath;
        }
    }

    private void OnDisable()
    {
        if (bossHealth != null)
        {
            bossHealth.OnDamaged -= HandleDamaged;
            bossHealth.OnDeath -= HandleDeath;
        }
    }

    private void HandleDamaged(Vector2 knockbackDir, float knockbackForce)
    {
        if (CurrentState == DeadState) return;

        bossController.ApplyKnockback(knockbackDir, knockbackForce);
        ChangeState(HurtState);
    }

    private void HandleDeath()
    {
        ChangeState(DeadState);
    }
}