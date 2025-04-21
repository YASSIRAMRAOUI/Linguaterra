using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsonantEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float moveDistance = 3f;
    public float startPositionOffset = 0f;
    public MoveDirection moveDirection = MoveDirection.Horizontal; // New: Movement direction
    public float movePauseTime = 0f; // Pause at ends of movement
    private Vector3 initialPosition;
    private Vector3 minPosition;
    private Vector3 maxPosition;
    private bool movingForward = true; // Renamed for clarity
    private float movePauseTimer = 0f;

    [Header("Interaction Settings")]
    public string consonantSound = "default_consonant"; // Customizable sound
    public float playerDescentSpeed = 5f;
    public string playerTag = "Player";
    public float deathDelay = 0.2f;
    public GameObject deathEffectPrefab;

    private Rigidbody2D rb;
    private Collider2D enemyCollider;
    private bool isDead = false;
    private Animator animator; // Optional Animator

    public enum MoveDirection
    {
        Horizontal,
        Vertical
    }

    void Start()
    {
        initialPosition = transform.position + GetDirectionVector() * startPositionOffset; // Use GetDirectionVector
        minPosition = initialPosition - GetDirectionVector() * moveDistance / 2f;
        maxPosition = initialPosition + GetDirectionVector() * moveDistance / 2f;
        transform.position = initialPosition;

        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>(); // Get Animator (optional)

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D missing on enemy: " + gameObject.name);
        }
        if (enemyCollider == null)
        {
            Debug.LogError("Collider2D missing on enemy: " + gameObject.name);
        }
    }

    void Update()
    {
        if (!isDead)
        {
            Move(); // Renamed MoveHorizontally to Move for generality
        }
    }

    void Move() // Renamed and generalized
    {
        if (movePauseTimer > 0)
        {
            movePauseTimer -= Time.deltaTime;
            rb.velocity = Vector2.zero; // Stop movement during pause
            return;
        }

        Vector3 moveVector = GetDirectionVector() * moveSpeed * Time.deltaTime * (movingForward ? 1 : -1);
        transform.position += moveVector;

        if (moveDirection == MoveDirection.Horizontal)
        {
            if ((movingForward && transform.position.x > maxPosition.x) ||
                (!movingForward && transform.position.x < minPosition.x))
            {
                movingForward = !movingForward;
                movePauseTimer = movePauseTime;
            }
        }
        else if (moveDirection == MoveDirection.Vertical)
        {
            if ((movingForward && transform.position.y > maxPosition.y) ||
                (!movingForward && transform.position.y < minPosition.y))
            {
                movingForward = !movingForward;
                movePauseTimer = movePauseTime;
            }
        }
    }

    Vector3 GetDirectionVector() // Helper function for direction
    {
        return moveDirection == MoveDirection.Horizontal ? Vector3.right : Vector3.up;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag(playerTag))
        {
            float collisionPoint = moveDirection == MoveDirection.Horizontal ? collision.contacts[0].point.y : collision.contacts[0].point.x;
            float enemyExtremePoint = moveDirection == MoveDirection.Horizontal ? enemyCollider.bounds.max.y : enemyCollider.bounds.max.x;
            float playerVelocity = moveDirection == MoveDirection.Horizontal ? collision.relativeVelocity.y : collision.relativeVelocity.x;

            if (collisionPoint > enemyExtremePoint - 0.1f && playerVelocity < 0)
            {
                HandleEnemyDeath(collision);
            }
        }
    }

    void HandleEnemyDeath(Collision2D collision)
    {
        isDead = true;
        Debug.Log("Player interacted with " + gameObject.name + ", enemy dies!");
        AudioManager.instance?.PlaySoundEffect(consonantSound);

        Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            Vector2 pushDirection = moveDirection == MoveDirection.Horizontal ? Vector2.down : moveDirection == MoveDirection.Vertical ? Vector2.left : Vector2.zero;
            playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y) + pushDirection * playerDescentSpeed; // Apply direction
        }

        enemyCollider.enabled = false;
        rb.velocity = Vector2.zero;

        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        if (animator != null)
        {
            animator.SetTrigger("Die"); // Trigger death animation if available
        }

        Destroy(gameObject, deathDelay);
    }
}