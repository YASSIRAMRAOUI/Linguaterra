using UnityEngine;
using System.Collections;
public class PlayerMovement3 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    [Tooltip("Minimum speed to trigger walking animation")]
    public float animationThreshold = 0.1f;
    private bool canMove = true; // Add this to control movement

    [Header("Audio Settings")]
    public GameObject GameManager;
    private AudioSource audioSource;
    private bool isAudioPlaying = false;

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

        // Audio setup
        if (GameManager != null)
        {
            audioSource = GameManager.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                StartCoroutine(WaitForAudioToFinish());
            }
            else
            {
                Debug.LogError("AudioSource non trouvé sur le GameManager !");
            }
        }

        if (rb == null) Debug.LogError("🚨 Rigidbody2D manquant");
        if (animator == null) Debug.LogError("🚨 Animator manquant");

        animator.SetBool("isGrounded", isGrounded);
    }

    void Update()
    {
        if (isAudioPlaying || !canMove) return; // Add canMove check

        HandleInput();
        HandleJump();
    }

    void FixedUpdate()
    {
        if (!isAudioPlaying && canMove) // Add canMove check
        {
            HandleMovement();
        }
        else if (!canMove && rb != null) 
        {
            // Stop movement when disabled
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetFloat("Speed", 0);
        }
    }

    // Add these two methods to control movement
    public void EnableMovement()
    {
        canMove = true;
        Debug.Log("Player movement enabled");
    }

    public void DisableMovement()
    {
        canMove = false;
        Debug.Log("Player movement disabled");
        
        // Stop movement immediately
        if (rb != null)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetFloat("Speed", 0);
        }
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
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetButtonDown("Jump")) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("Jump");
            isGrounded = false;
            animator.SetBool("isGrounded", isGrounded);
        }
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