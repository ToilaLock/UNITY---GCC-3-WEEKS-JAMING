using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Boss Settings")]
    [SerializeField] private int bossHealth = 200;
    [SerializeField] private float detectRange = 8f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float attackCooldown = 1.5f;
    [HideInInspector] public int BossHealth => bossHealth;

    [Header("HitBox Reference")]
    [SerializeField] private Collider2D attackHitBox;
    [SerializeField] private Transform playerTarget;

    [Header("Knockback Settings")]
    [SerializeField] private float knockbackResistance = 0.5f;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public BossAnimator bossAnimator;
    [HideInInspector] public Transform playerTransform => playerTarget;

    [HideInInspector] public BossStateMachine StateMachine { get; private set; }

    [HideInInspector] public float AttackCooldown => attackCooldown;
    [HideInInspector] public float LastAttackTime { get; set; } = -99f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bossAnimator = GetComponent<BossAnimator>();
        StateMachine = GetComponent<BossStateMachine>();

        if (playerTransform == null) Debug.Log("PLAYER TARGET CANNOT DETECT");
    }

    // MOVE & FLIP
    public void MoveTowardsPlayer()
    {
        if (playerTransform == null) return;

        float direction = Mathf.Sign(playerTransform.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        Flip(direction);
    }

    public void StopMovement()
    {
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    public void Flip(float direction)
    {
        float scaleX = Mathf.Abs(transform.localScale.x);
        if (direction > 0)
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        else if (direction < 0)
            transform.localScale = new Vector3(-scaleX, transform.localScale.y, transform.localScale.z);
    }


    // ATTACK
    public float GetDistanceToPlayer()
    {
        if (playerTarget == null || !playerTarget.gameObject.activeInHierarchy)
            return float.MaxValue;

        return Vector2.Distance(transform.position, playerTransform.position);
    }

    public bool IsPlayerInDetectRange() => GetDistanceToPlayer() <= detectRange;
    public bool IsPlayerInAttackRange() => GetDistanceToPlayer() <= attackRange;
    public bool CanAttack() => Time.time - LastAttackTime >= attackCooldown;

    public void EnableHitBox()
    {
        if (attackHitBox != null) attackHitBox.enabled = true;
    }

    public void DisableHitBox()
    {
        if (attackHitBox != null) attackHitBox.enabled = false;
    }


    // ANIMATION EVENT
    public void OnAttackAnimationFinished()
    {
        if (StateMachine != null && StateMachine.AttackState != null)
        {
            StateMachine.AttackState.OnAttackAnimationFinished();
        }
    }

    // HITTED
    public void ApplyKnockback(Vector2 knockbackDir, float knockbackForce)
    {
        rb.linearVelocity = Vector2.zero;
        float finalForce = knockbackForce * (1f - Mathf.Clamp01(knockbackResistance));
        Vector2 force = new Vector2(knockbackDir.x, 0.2f).normalized * finalForce;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}