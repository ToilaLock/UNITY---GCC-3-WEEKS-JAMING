using UnityEngine;

public class PlayerAttackState : IState
{
    private PlayerStateMachine stateMachine;
    private PlayerInputHandler inputHandler;
    private PlayerController playerController;
    private PlayerAudio playerAudio;


    [SerializeField] private float ResetTime = 0.8f;
    private int comboIndex = 1;
    private float lastAttackEndTime;

    private bool canContinueAttack = false;
    private bool attackContinue = false;

    public PlayerAttackState(
        PlayerStateMachine stateMachine,
        PlayerInputHandler inputHandler,
        PlayerController playerController,
        PlayerAudio playerAudio)
    {
        this.stateMachine = stateMachine;
        this.inputHandler = inputHandler;
        this.playerController = playerController;
        this.playerAudio = playerAudio;
    }

    public void Enter()
    {
        if (Time.time - lastAttackEndTime > ResetTime || comboIndex > 3)
        {
            comboIndex = 1;
        }

        Attack();

        canContinueAttack = false;
        attackContinue = false;

        inputHandler.AttackPressed += HandleAttackInput;
    }

    public void Exit()
    {
        lastAttackEndTime = Time.time;
        comboIndex = 1;
        playerController.DisableHitBox();

        inputHandler.AttackPressed -= HandleAttackInput;
    }

    public void Update()
    {
        if (!playerController.isGrounded)
        {
            stateMachine.ChangeState(stateMachine.FallState);
        }
        else if (Mathf.Abs(inputHandler.MoveInput.x) > 0.05f)
            stateMachine.ChangeState(stateMachine.MoveState);
    }

    public void FixedUpdate() { }


    private void Attack()
    {
        playerController.StopMovement(); 
        switch(comboIndex)
        {
            case 1: playerAudio.PlayAttack1Sound(); break;
            case 2: playerAudio.PlayAttack2Sound(); break;
            case 3: playerAudio.PlayAttack3Sound(); break;
            default: playerAudio.PlayAttack1Sound(); break;
        }
        playerController.playerAnimator.PlayerAttack(comboIndex); 
    }


    public void EnableAttackContinue()
    {
        canContinueAttack = true;
       
        if (attackContinue)
        {
            AdvanceCombo();
        }
    }


    private void HandleAttackInput()
    {
        if (canContinueAttack)
        {
            AdvanceCombo();
        }
        else
        {
            attackContinue = true;
        }
    }


    private void AdvanceCombo()
    {
        if (comboIndex < 3)
        {
            comboIndex++;
            canContinueAttack = false;
            attackContinue = false;
            playerController.DisableHitBox();

            Attack();
        }
    }

    // EXIT ANIM
    public void OnAnimationFinished()
    {
        comboIndex = 1;

        if (!playerController.isGrounded)
        {
            stateMachine.ChangeState(stateMachine.FallState);
        }
        else if (Mathf.Abs(inputHandler.MoveInput.x) > 0.05f)
            stateMachine.ChangeState(stateMachine.MoveState);
        else
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }      
}

