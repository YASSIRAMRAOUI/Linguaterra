using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1f;
    public float jumpForce = 1f;
    [Tooltip("Minimum speed to trigger walking animation")]
    public float animationThreshold = 0.1f;

    [Header("Component References")]
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded = true;

    [HideInInspector] public Vector3 respawnPosition;

    private float moveInput = 0f;
    private bool jumpPressed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (rb == null) Debug.LogError("🚨 Rigidbody2D manquant sur " + gameObject.name);
        if (animator == null) Debug.LogError("🚨 Animator manquant sur " + gameObject.name);

        respawnPosition = transform.position;
        animator.SetBool("isGrounded", isGrounded);
    }

    void Update()
    {
        HandleJump();

        // Update animation and flip based on input
        if (Mathf.Abs(moveInput) > animationThreshold) // Ensure the speed is above the threshold to trigger movement
        {
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
            // Flip the sprite based on movement direction
            transform.localScale = new Vector3(Mathf.Sign(moveInput) * 0.05f, 0.05f, 1f);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // Apply movement on the X-axis based on input, keeping the Y-axis velocity (gravity) unaffected
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void HandleJump()
    {
        if (jumpPressed && isGrounded)
        {
            // Apply vertical velocity for the jump
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("Jump");
            isGrounded = false;
            animator.SetBool("isGrounded", isGrounded);
        }

        // Reset jump flag after applying jump
        jumpPressed = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isGrounded", isGrounded);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("isGrounded", isGrounded);
        }
    }

    public void RespawnPlayer()
    {
        transform.position = respawnPosition;
        rb.velocity = Vector3.zero;
        isGrounded = true;
        animator.SetBool("isGrounded", isGrounded);
    }

    public void SetRespawnPosition(Vector3 newRespawnPosition)
    {
        respawnPosition = newRespawnPosition;
    }

    // ✅ UI Button Methods
    public void MoveLeft() => moveInput = -1f;  // Move left when button pressed
    public void MoveRight() => moveInput = 1f;  // Move right when button pressed
    public void StopMoving() => moveInput = 0f; // Stop movement when button released
    public void JumpButtonPressed() => jumpPressed = true;  // Jump when button pressed
}
