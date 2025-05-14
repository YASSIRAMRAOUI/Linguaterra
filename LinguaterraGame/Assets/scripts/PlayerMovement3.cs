using UnityEngine;
using System.Collections;

public class PlayerMovement3 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float animationThreshold = 0.1f;
    private bool canMove = true;

    [Header("Audio Settings")]
    public GameObject GameManager;
    private AudioSource audioSource;
    private bool isAudioPlaying = false;

    [Header("Component References")]
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded = true;
    private Vector3 originalScale;

    private float moveInput = 0f;
    private bool jumpPressed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;

        if (GameManager != null)
        {
            audioSource = GameManager.GetComponent<AudioSource>();
            if (audioSource != null)
                StartCoroutine(WaitForAudioToFinish());
            else
                Debug.LogError("AudioSource not found on GameManager!");
        }

        if (rb == null) Debug.LogError("🚨 Rigidbody2D missing");
        if (animator == null) Debug.LogError("🚨 Animator missing");

        animator.SetBool("isGrounded", isGrounded);
    }

    void Update()
    {
        if (isAudioPlaying || !canMove) return;

        HandleJump();

        // Animate and flip sprite
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        if (moveInput > 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    void FixedUpdate()
    {
        if (!isAudioPlaying && canMove)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetFloat("Speed", 0);
        }
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
        jumpPressed = false;
    }

    private IEnumerator WaitForAudioToFinish()
    {
        isAudioPlaying = true;
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
        }
        isAudioPlaying = false;
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

    public void RespawnPlayer(Vector3 position)
    {
        Debug.Log("Respawning player at: " + position);
        rb.velocity = Vector2.zero;
        transform.position = position;
        isGrounded = true;
        animator.SetBool("isGrounded", isGrounded);
    }

    // Mobile Button Controls
    public void MoveLeft() => moveInput = -1f;
    public void MoveRight() => moveInput = 1f;
    public void StopMoving() => moveInput = 0f;
    public void JumpButtonPressed() => jumpPressed = true;

    public void EnableMovement()
    {
        canMove = true;
        Debug.Log("Movement enabled");
    }

    public void DisableMovement()
    {
        canMove = false;
        Debug.Log("Movement disabled");
        if (rb != null)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetFloat("Speed", 0);
        }
    }
}
