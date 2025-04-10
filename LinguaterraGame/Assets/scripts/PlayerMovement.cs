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
    private bool isGrounded = true; // Initialize to false
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
    }

    void Update()
    {
        HandleInput();
        HandleJump();
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

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput > 0)
        {
            transform.localScale = new Vector3(0.28f, 0.3f, 1f);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-0.28f, 0.3f, 1f);
        }
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
        }
        else
        {
            isGrounded = false; // Reset grounded state if not colliding with ground
        }
    }


}