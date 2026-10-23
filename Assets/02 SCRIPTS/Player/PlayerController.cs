using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Setting")]
    [SerializeField] public int HealthPoint = 100; 
    [SerializeField] public float Stamina = 50;


    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float coyoteTime = 0.2f;
    public float CoyoteTime => coyoteTime;


    [Header("Raycast Settings")]
    [SerializeField] private float groundCheckDist = 0.1f;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private LayerMask groundLayer;
    public bool isGrounded;
    public bool isFalling => rb.linearVelocity.y < 0 && !isGrounded;
    public float verticalVelocity => rb.linearVelocity.y;


    [Header("Dash Setting")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashStamina = 20;
    public float DashDuration => dashDuration;
    public float DashStamina => dashStamina;


    [Header("Roll Setting")]
    [SerializeField] private float rollSpeed = 8f;
    [SerializeField] private float rollDuration = 0.35f;
    [SerializeField] private float rollStaminia = 15;
    public float RollDuration => rollDuration;
    public float RollStamina => rollStaminia;
    

    [Header("Hit Box")]
    [SerializeField] private Collider2D attackHitBox;

    // DASH & ROLL
    private float defaultGravity;
    public float FaceDirection => Mathf.Sign(transform.localScale.x);

    public Rigidbody2D rb;
    public PlayerAnimator playerAnimator; 
    private PlayerInputHandler inputHandler;
    private SpriteRenderer spriteRenderer;

    public PlayerHealth HealthComponent { get; private set; }
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputHandler = GetComponent<PlayerInputHandler>();
        playerAnimator = GetComponent<PlayerAnimator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        HealthComponent = GetComponent<PlayerHealth>();

        defaultGravity = rb.gravityScale;
        isGrounded = true;
    }

    private void FixedUpdate() 
    {
        GroundCheck();
    }

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.down * groundCheckDist, groundCheckSize);
    }


    //DASH STRUCTURE
    public void StartDash()
    {
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(FaceDirection * dashSpeed, 0f);
    }

    public void StopDash()
    {
        rb.gravityScale = defaultGravity;
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }


    // ROLL STRUCTURE
    public void Roll()
    {
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
        Stamina = Mathf.Max(0f, Stamina - amount);

        if(Stamina <= 0) 
        {
            Debug.Log("MANA END");
            return;
        }

        Debug.Log($"Mana: {Stamina}");
    }

    public bool HasStamina()
    {
        return Stamina > 0;
    }


    //HIT BOX
    public void EnableHitBox()
    {
        if (attackHitBox != null) attackHitBox.enabled = true;
        Debug.Log("Enable HitBox");
    }

    public void DisableHitBox()
    {
        if(attackHitBox != null) attackHitBox.enabled = false;
        Debug.Log("Disable HitBox");
    }

    
    // KNOCK BACK
    public void ApplyKnockback(Vector2 knockbackDir, float knockbackForce)
    {
        rb.linearVelocity = Vector2.zero;

        Vector2 force = new Vector2(knockbackDir.x, 0.5f).normalized * knockbackForce;
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}