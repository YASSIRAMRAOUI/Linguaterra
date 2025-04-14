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
    [Tooltip("Animator component for player animations")]
    private bool isGrounded = true; // Initialize to true!  Important change
    private Vector3 respawnPosition = new Vector3(-3.5f, 0.5f,-25.2f); // Default respawn position

    void Start()
    {
        // Get required components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Error handling if components are missing
        if (rb == null)
        {
            Debug.LogError("🚨 Rigidbody2D manquant sur " + gameObject.name);
        }

        if (animator == null)
        {
            Debug.LogError("🚨 Animator manquant sur " + gameObject.name);
        }

        animator.SetBool("isGrounded", isGrounded); // Initialize animator's isGrounded
    }

    void Update()
    {
        HandleInput();
        HandleJump();
    }

    // FixedUpdate is better for physics
    void FixedUpdate()
    {
        HandleMovement(); // Move the movement logic to FixedUpdate
    }

    void HandleInput()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(moveInput) > 0)
        {
            animator.SetFloat("Speed", Mathf.Abs(moveInput)); // Set speed for animation
        }
        else
        {
            animator.SetFloat("Speed", 0); // Reset speed if no input
        }

        //  The velocity is set in FixedUpdate now
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(0.28f, 0.3f, 1f);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-0.28f, 0.3f, 1f);
        }
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void HandleJump()
    {
        // Jump if grounded and jump button pressed
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("Jump");
            isGrounded = false; // Player is no longer grounded after jumping
            animator.SetBool("isGrounded", isGrounded); // Update animator after jump logic
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isGrounded", isGrounded); // Update animator
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("isGrounded", isGrounded); // Update animator
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // Use OnTrigger for respawn
    {
        if (collision.CompareTag("respawn"))
        {
            RespawnPlayer(respawnPosition);
        }
    }

    public void RespawnPlayer(Vector3 respawnPosition) // Corrected method name
    {
        transform.position = respawnPosition;
        rb.velocity = Vector3.zero; // Reset velocity on respawn
        isGrounded = true; // Reset grounded state on respawn
        animator.SetBool("isGrounded", isGrounded); // Update animator after respawn
    }
}