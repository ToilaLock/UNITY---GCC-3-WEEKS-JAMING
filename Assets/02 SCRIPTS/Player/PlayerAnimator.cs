using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    private static readonly int IdleHash = Animator.StringToHash("Player_Idle");
    private static readonly int RunHash = Animator.StringToHash("Player_Run");
    private static readonly int JumpHash = Animator.StringToHash("Player_Jump");
    private static readonly int FallHash = Animator.StringToHash("Player_Fall");

    // private static readonly int DashHash = Animator.StringToHash("Player_Dash");
    // private static readonly int AttackHash = Animator.StringToHash("Player_Attack");
    // private static readonly int BlockHash = Animator.StringToHash("Player_Block");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayIdle() => animator.Play(IdleHash);
    public void PlayRun() => animator.Play(RunHash);
    public void PlayJump() => animator.Play(JumpHash);
    public void PlayFall() => animator.Play(FallHash);

    // public void PlayDash() => animator.Play(DashHash);
    // public void PlayAttack() => animator.Play(AttackHash);
    // public void PlayBlock() => animator.Play(BlockHash);
}