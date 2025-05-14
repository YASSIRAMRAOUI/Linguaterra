using UnityEngine;

public class PlayerMovement4 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 20f;
    public float jumpForce = 10f;
    public float animationThreshold = 0.1f;

    [Header("Component References")]
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded = true;
    private Vector3 respawnPosition = new Vector3(-3.5f, 0.5f, -25.2f);

    [Header("Control Settings")]
    public bool canMove = false;

    // Mobile Control variables
    private float moveInput = 0f;
    private bool jumpPressed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (rb == null) Debug.LogError("🚨 Rigidbody2D manquant !");
        if (animator == null) Debug.LogError("🚨 Animator manquant !");

        animator.SetBool("isGrounded", isGrounded);
    }

    void Update()
    {
        if (!canMove) return;

        HandleInput();
        HandleJump();
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        HandleMovement();
    }

    void HandleInput()
    {
        // Use moveInput for controlled movement
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        if (moveInput > 0)
            animator.SetBool("isFacingRight", true);
        else if (moveInput < 0)
            animator.SetBool("isFacingRight", false);
    }

    void HandleMovement()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void HandleJump()
    {
        if (jumpPressed && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("Jump");
            isGrounded = false;
            animator.SetBool("isGrounded", isGrounded);
        }

        jumpPressed = false; // Reset jump flag
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("💥 Collision détectée avec : " + collision.gameObject.name + " (Tag: " + collision.gameObject.tag + ")");

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isGrounded", isGrounded);
            Debug.Log("✅ Le joueur est maintenant au sol.");
        }

        if (collision.gameObject.CompareTag("Animal"))
        {
            Debug.Log("🐾 Collision avec un animal !");
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

    public void RespawnPlayer(Vector3 pos)
    {
        transform.position = pos;
        rb.velocity = Vector3.zero;
        isGrounded = true;
        animator.SetBool("isGrounded", isGrounded);
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    // MOBILE CONTROL METHODS - Movement starts when the button is pressed
    public void MoveLeft()
    {
        moveInput = -1f;  // Start moving left
    }

    public void MoveRight()
    {
        moveInput = 1f;  // Start moving right
    }

    public void StopMoving()
    {
        moveInput = 0f;  // Stop movement
    }

    public void JumpButtonPressed()
    {
        jumpPressed = true;  // Set jump flag when jump button is pressed
    }
}
