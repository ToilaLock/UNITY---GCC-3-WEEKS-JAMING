using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private PlayerInputHandler inputHandler;
    private StateMachine stateMachine;
    private PlayerController playerController;


    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }

    public PlayerAttackState AttackState { get; private set; }
    public PlayerRollState RollState { get; private set; }

    public PlayerHurtState HurtState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }


    private void Awake()
    {
        stateMachine = new StateMachine();
        inputHandler = GetComponent<PlayerInputHandler>();
        playerController = GetComponent<PlayerController>();
        
        IdleState = new PlayerIdleState(this, inputHandler, playerController);
        MoveState = new PlayerMoveState(this, inputHandler, playerController);
        JumpState = new PlayerJumpState(this, inputHandler, playerController);
        FallState = new PlayerFallState(this, inputHandler, playerController);

        AttackState = new PlayerAttackState(this, inputHandler, playerController);
        RollState = new PlayerRollState(this, inputHandler, playerController);

        HurtState = new PlayerHurtState(this, playerController);
        DeadState = new PlayerDeadState(playerController);
    }

    private void Start() => stateMachine.Initialize(IdleState);
    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();
    public void ChangeState(IState newState) {
        if (stateMachine.CurrentState == DeadState) return;

        stateMachine.ChangeState(newState);
    }

    private void OnEnable() 
    {
        inputHandler.JumpPressed += OnJumpPressed;
        inputHandler.RollPressed += OnRollPressed;
        inputHandler.AttackPressed += OnAttackPressed;

        playerController.HealthComponent.OnDamaged += OnDamaged;
        playerController.HealthComponent.OnDeath += OnDeath;
    }
    private void OnDisable()
    {
        inputHandler.JumpPressed -= OnJumpPressed;
        inputHandler.RollPressed -= OnRollPressed;
        inputHandler.AttackPressed -= OnAttackPressed;

        playerController.HealthComponent.OnDamaged -= OnDamaged;
        playerController.HealthComponent.OnDeath -= OnDeath;
    }

    private void OnJumpPressed()
    {
        if (stateMachine.CurrentState == DeadState || 
            stateMachine.CurrentState == HurtState ||
            stateMachine.CurrentState == JumpState)
            return;

        if (playerController.isGrounded)
        {
            stateMachine.ChangeState(JumpState);
            return;
        }

        if (stateMachine.CurrentState == FallState && FallState.CanCoyoteJump())
        {
            stateMachine.ChangeState(JumpState);
        }
    }

    private void OnRollPressed()
    {
        if (RollState.CanRoll())
        {
            if (stateMachine.CurrentState == DeadState) return;
            stateMachine.ChangeState(RollState);
        }
    }

    private void OnAttackPressed()
    {

        stateMachine.ChangeState(AttackState);
    }

    
    // DAMAGE Take
    private void OnDamaged(Vector2 knockBackDir, float knockBackForce)
    {
        if (stateMachine.CurrentState == DeadState) return;

        playerController.ApplyKnockback(knockBackDir, knockBackForce);

        stateMachine.ChangeState(HurtState);
    }

    private void OnDeath()
    {
        stateMachine.ChangeState(DeadState);
    }
}