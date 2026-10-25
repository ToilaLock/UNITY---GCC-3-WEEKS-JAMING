using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Setting")]
    [SerializeField] public int HealthPoint = 100; 
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaRegenRate = 20f; 
    [SerializeField] private float staminaRegenDelay = 0.8f; 
    [HideInInspector] public float CurrentStamina { get; private set; }
    [HideInInspector] public float MaxStamina => maxStamina;
    [HideInInspector] private float regenDelayTimer;
    [HideInInspector] public event System.Action<float, float> OnStaminaChanged;


    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float coyoteTime = 0.2f;
    [HideInInspector] public float CoyoteTime => coyoteTime;


    [Header("Raycast Settings")]
    [SerializeField] private float groundCheckDist = 0.1f;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private LayerMask groundLayer;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isFalling => rb.linearVelocity.y < 0 && !isGrounded;
    [HideInInspector] public float verticalVelocity => rb.linearVelocity.y;


    [Header("Roll Setting")]
    [SerializeField] private float rollSpeed = 8f;
    [SerializeField] private float rollDuration = 0.35f;
    [SerializeField] private float rollStaminia = 15;
    [HideInInspector] public float RollDuration => rollDuration;
    [HideInInspector] public float RollStamina => rollStaminia;
    

    [Header("Hit Box")]
    [SerializeField] private Collider2D attackHitBox;

    // DASH & ROLL
    private float defaultGravity;
    [HideInInspector] public float FaceDirection => Mathf.Sign(transform.localScale.x);

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public PlayerAnimator playerAnimator; 
    private PlayerInputHandler inputHandler;
    private PlayerAudio playerAudio;
    private SpriteRenderer spriteRenderer;

    [HideInInspector] public PlayerHealth HealthComponent { get; private set; }
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputHandler = GetComponent<PlayerInputHandler>();
        playerAnimator = GetComponent<PlayerAnimator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        HealthComponent = GetComponent<PlayerHealth>();
        playerAudio = GetComponent<PlayerAudio>();

        defaultGravity = rb.gravityScale;
        isGrounded = true;

        CurrentStamina = maxStamina;
    }

    private void Update() => HandleStaminaRegen();

    private void FixedUpdate() 
    {
        GroundCheck();
    }


    // MOVE, FLIP & JUMP
    public void Move()
    {
        Vector2 moveInput = inputHandler.MoveInput;
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    public void StopMovement()
    {
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }
    
    public void Flip()
    {
        float scaleX = Mathf.Abs(transform.localScale.x);
        Vector2 moveInput = inputHandler.MoveInput;
        if (moveInput.x > 0)
            transform.localScale = new Vector3(scaleX, scaleX, scaleX);
        else if (moveInput.x < 0)
            transform.localScale = new Vector3(-scaleX, scaleX, scaleX);
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void GroundCheck()
    {
        isGrounded = Physics2D.BoxCast(transform.position, groundCheckSize, 0f, Vector3.down, groundCheckDist, groundLayer);
    }

    // ROLL STRUCTURE
    public void Roll()
    {
        UseStamina(RollStamina);
        if (CurrentStamina <= 0) {
            return;
        }
        rb.linearVelocity = new Vector2(FaceDirection* rollSpeed, rb.linearVelocity.y);
    }

    public void SetInvincibleVisual(bool isInvincible)
    {
        if (spriteRenderer == null) return;

        Color c = spriteRenderer.color;
        c.a = isInvincible ? 0.5f : 1f;
        spriteRenderer.color = c;
    }


    //HP & STAMINA
    public void UseStamina(float amount)
    {
        CurrentStamina = Mathf.Max(0f, CurrentStamina - amount);
        regenDelayTimer = staminaRegenDelay; 
        OnStaminaChanged?.Invoke(CurrentStamina, maxStamina);

        if(CurrentStamina <= 0) 
        {
            HealthComponent.IsInvincible = false;
            HealthComponent.TakeDamage(1000, Vector2.zero, 0f);
            Debug.Log("DEAD BECAUSE MANA ZERO");
            return;
        }

        Debug.Log($"Mana: {CurrentStamina}");
    }

    private void HandleStaminaRegen()
    {
        if (CurrentStamina <= 0f) return;

        if (regenDelayTimer > 0f)
        {
            regenDelayTimer -= Time.deltaTime;
            return;
        }

        if (CurrentStamina < maxStamina)
        {
            CurrentStamina = Mathf.Min(maxStamina, CurrentStamina + staminaRegenRate * Time.deltaTime);
            OnStaminaChanged?.Invoke(CurrentStamina, maxStamina);
        }
    }

    public bool HasStamina()
    {
        return CurrentStamina > 0;
    }


    //HIT BOX
    public void EnableHitBox()
    {
        if (attackHitBox != null) attackHitBox.enabled = true;
    }

    public void DisableHitBox()
    {
        if(attackHitBox != null) attackHitBox.enabled = false;
    }

    
    // KNOCK BACK
    public void ApplyKnockback(Vector2 knockbackDir, float knockbackForce)
    {
        rb.linearVelocity = Vector2.zero;

        Vector2 force = new Vector2(knockbackDir.x, 0.5f).normalized * knockbackForce;
        rb.AddForce(force, ForceMode2D.Impulse);
    }


    // GIZMOS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.down * groundCheckDist, groundCheckSize);
    }
}