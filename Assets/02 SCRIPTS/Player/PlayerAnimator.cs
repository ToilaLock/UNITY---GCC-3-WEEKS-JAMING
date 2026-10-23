using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    private PlayerStateMachine stateMachine;
    private PlayerController playerController;

    private static readonly int IdleHash = Animator.StringToHash("Player_Idle");
    private static readonly int RunHash = Animator.StringToHash("Player_Run");
    private static readonly int JumpHash = Animator.StringToHash("Player_Jump");
    private static readonly int FallHash = Animator.StringToHash("Player_Fall");

    private static readonly int RollHash = Animator.StringToHash("Player_Roll");
    private static readonly int BlockHash = Animator.StringToHash("Player_Block");

    private static readonly int Attack1Hash = Animator.StringToHash("Player_Attack_1");
    private static readonly int Attack2Hash = Animator.StringToHash("Player_Attack_2");
    private static readonly int Attack3Hash = Animator.StringToHash("Player_Attack_3");


    private void Awake()
    {
        animator = GetComponent<Animator>();
        stateMachine = GetComponent<PlayerStateMachine>();
        playerController = GetComponent<PlayerController>();
    }

    // RUN ANIM
    public void PlayIdle() => animator.Play(IdleHash);
    public void PlayRun() => animator.Play(RunHash);
    public void PlayJump() => animator.Play(JumpHash);
    public void PlayFall() => animator.Play(FallHash);

    public void PlayRoll() => animator.Play(RollHash);
    public void PlayBlock() => animator.Play(BlockHash);

    public void PlayerAttack(int comboIndex)
    {
        switch (comboIndex)
        {
            case 1: animator.Play(Attack1Hash); break;
            case 2: animator.Play(Attack2Hash); break;
            case 3: animator.Play(Attack3Hash); break;
            default: animator.Play(Attack1Hash); break;
        }
    }


    // ANIM BRIGDE
    public void AnimEvent_OpenHitbox() => playerController.EnableHitBox();
    public void AnimEvent_CloseHitbox() => playerController.DisableHitBox();

    public void AnimEvent_EnableComboWindow()
    {
        stateMachine.AttackState.EnableAttackContinue();
    }

    public void AnimEvent_OnAttackFinished()
    {
        stateMachine.AttackState.OnAnimationFinished();
    }
}