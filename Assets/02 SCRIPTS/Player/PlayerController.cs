using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Raycast Settings")]
    [SerializeField] private float groundCheckDist = 0.1f;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;

    private Rigidbody2D rb;
    private PlayerInputHandler inputHandler;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputHandler = GetComponent<PlayerInputHandler>();

        isGrounded = true;
    }

    private void FixedUpdate() // Should use FixedUpdate for physics-related updates
    {
        Move();
        Flip();
        GroundCheck();

        if (inputHandler.JumpHold && isGrounded)
        {
            Jump();
        }
    }

    private void OnDisable()
    {
        inputHandler.JumpPressed -= Jump;
    }

    private void Move()
    {
        Vector2 moveInput = inputHandler.MoveInput;
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed,rb.linearVelocity.y);
    }

    private void Flip()
    {
        float scaleX = Mathf.Abs(transform.localScale.x);
        Vector2 moveInput = inputHandler.MoveInput;
        if(moveInput.x > 0)
        {
            transform.localScale = new Vector3(scaleX * 1, scaleX * 1, scaleX * 1);
            // Debug.Log("Facing Right");
        }
        else if(moveInput.x < 0)
        {     
            transform.localScale = new Vector3(scaleX * -1, scaleX * 1, scaleX * 1);
            // Debug.Log("Facing Left");
        }
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

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        // Debug.Log("Jumping");
    }
}