using UnityEngine;

public class BossAnimator : MonoBehaviour
{
    private Animator animator;
    private BossController bossController;

    private static readonly int IdleHash = Animator.StringToHash("Boss_Idle");
    private static readonly int WalkHash = Animator.StringToHash("Boss_Walk");
    private static readonly int AttackHash = Animator.StringToHash("Boss_Attack");

    private static readonly int TakeDamageHash = Animator.StringToHash("Boss_TakeDamage");
    private static readonly int DeadHash = Animator.StringToHash("Boss_Dead");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        bossController = GetComponent<BossController>();
    }

    public void PlayIdle() => animator.Play(IdleHash, 0, 0f);
    public void PlayWalk() => animator.Play(WalkHash);
    public void PlayAttack() => animator.Play(AttackHash, 0, 0f);

    public void PlayerTakeDamage() => animator.Play(TakeDamageHash);
    public void PlayDead() => animator.Play(DeadHash);

    // ANIMATION EVENT
    public void AnimEvent_EnableHitBox() => bossController.EnableHitBox();
    public void AnimEvent_DisableHitBox() => bossController.DisableHitBox();
    public void AnimEvent_OnAttackFinished() => bossController.OnAttackAnimationFinished();
    public void AnimEvent_Destroy() => Destroy(gameObject);
}