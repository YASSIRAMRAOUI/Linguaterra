using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    [Tooltip("Minimum speed to trigger walking animation")]
    public float animationThreshold = 0.1f;

    [Header("Component References")]
    private Rigidbody2D rb;
    private Animator animator;

    private Vector3 respawnPosition = new Vector3(-3.5f, 0.5f, -25.2f);
    private Vector3 originalScale;

    private float moveInput = 0f;
    private bool jumpPressed = false;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;

        if (rb == null) Debug.LogError("🚨 Rigidbody2D missing");
        if (animator == null) Debug.LogError("🚨 Animator missing");

        animator.SetBool("isGrounded", isGrounded);
    }

    void Update()
    {
        HandleMovementAnimation();
        HandleJump();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // Move the player horizontally while maintaining current vertical velocity (falling/jumping)
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    private void HandleMovementAnimation()
    {
        // Set the animation speed
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        // Flip the player sprite based on movement direction
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
    }

    private void HandleJump()
    {
        // Check if jump button is pressed and the player is on the ground
        if (jumpPressed && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Apply jump force
            animator.SetTrigger("Jump"); // Trigger the jump animation
            isGrounded = false;
            animator.SetBool("isGrounded", isGrounded); // Update grounded status
        }

        // Reset jump flag after using
        jumpPressed = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isGrounded", isGrounded); // Update grounded status on collision with the ground
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("isGrounded", isGrounded); // Update grounded status on leaving ground
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("respawn"))
        {
            RespawnPlayer(respawnPosition); // Trigger respawn when colliding with respawn point
        }
    }

    public void RespawnPlayer(Vector3 pos)
    {
        transform.position = pos; // Set the player position to respawn
        rb.velocity = Vector3.zero; // Reset velocity
        isGrounded = true; // Ensure player is grounded after respawn
        animator.SetBool("isGrounded", isGrounded); // Update grounded status
    }

    // 🔽 UI Button Methods to be called from buttons
    public void MoveLeft() => moveInput = -1f;  // Move left when button is pressed
    public void MoveRight() => moveInput = 1f;  // Move right when button is pressed
    public void StopMoving() => moveInput = 0f; // Stop movement when button is released
    public void JumpButtonPressed() => jumpPressed = true;  // Jump when button is pressed
}
