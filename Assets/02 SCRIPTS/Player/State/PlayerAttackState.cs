using UnityEngine;

public class PlayerAttackState : IState
{
    private PlayerStateMachine stateMachine;
    private PlayerInputHandler inputHandler;
    private PlayerController playerController;


    [SerializeField] private float ResetTime = 0.8f;
    private int comboIndex = 1;
    private float lastAttackEndTime;

    private bool canContinueAttack = false;
    private bool attackContinue = false;

    public PlayerAttackState(
        PlayerStateMachine stateMachine,
        PlayerInputHandler inputHandler,
        PlayerController playerController)
    {
        this.stateMachine = stateMachine;
        this.inputHandler = inputHandler;
        this.playerController = playerController;
    }

    public void Enter()
    {
        if (Time.time - lastAttackEndTime > ResetTime || comboIndex > 3)
        {
            comboIndex = 1;
        }

        playerController.StopMovement();
        playerController.playerAnimator.PlayerAttack(comboIndex);

        canContinueAttack = false;
        attackContinue = false;

        inputHandler.AttackPressed += HandleAttackInput;
    }

    public void Exit()
    {
        lastAttackEndTime = Time.time;
        comboIndex++;
        playerController.DisableHitBox();

        inputHandler.AttackPressed -= HandleAttackInput;
    }

    public void Update() {}

    public void FixedUpdate() {}

    public void EnableAttackContinue() => canContinueAttack = true;

    public void OnAnimationFinished()
    {
        if (attackContinue && comboIndex < 3)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
        }
        else
        {
            comboIndex = 1;

            if (!playerController.isGrounded)
            {
                stateMachine.ChangeState(stateMachine.FallState);
            }
            else if (Mathf.Abs(inputHandler.MoveInput.x) > 0.05f)
            {
                stateMachine.ChangeState(stateMachine.MoveState);
            }
            else
            {
                stateMachine.ChangeState(stateMachine.IdleState);
            }
        }
    }

    private void HandleAttackInput()
    {
        if (canContinueAttack)
        {
            attackContinue = true;
        }
    }
}
