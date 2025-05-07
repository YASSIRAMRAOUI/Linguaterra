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
    private bool isGrounded = true;
    private Vector3 respawnPosition = new Vector3(-3.5f, 0.5f, -25.2f);
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;

        if (rb == null) Debug.LogError("🚨 Rigidbody2D manquant");
        if (animator == null) Debug.LogError("🚨 Animator manquant");

        animator.SetBool("isGrounded", isGrounded);
    }

    void Update()
    {
        HandleInput();
        HandleJump();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleInput()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(moveInput) > 0)
        {
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

        if (moveInput > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("Jump");
            isGrounded = false;
            animator.SetBool("isGrounded", isGrounded);
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("respawn"))
        {
            RespawnPlayer(respawnPosition);
        }
    }

    public void RespawnPlayer(Vector3 respawnPosition)
    {
        transform.position = respawnPosition;
        rb.velocity = Vector3.zero;
        isGrounded = true;
        animator.SetBool("isGrounded", isGrounded);
    }
}
