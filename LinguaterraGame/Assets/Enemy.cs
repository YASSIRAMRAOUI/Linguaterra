using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsonantEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float moveDistance = 3f;
    public float startPositionOffset = 0f;
    public MoveDirection moveDirection = MoveDirection.Horizontal;
    public float movePauseTime = 0f;
    private Vector3 initialPosition;
    private Vector3 minPosition;
    private Vector3 maxPosition;
    private bool movingForward = true;
    private float movePauseTimer = 0f;

    [Header("Interaction Settings")]
    public string consonantSound = "default_consonant";
    public float playerDescentSpeed = 5f;
    public string playerTag = "Player";
    public float deathDelay = 0.2f;
    public GameObject deathEffectPrefab;

    private Rigidbody2D rb;
    private Collider2D enemyCollider;
    private bool isDead = false;
    private Animator animator;

    public enum MoveDirection
    {
        Horizontal,
        Vertical
    }

    void Start()
    {
        initialPosition = transform.position + GetDirectionVector() * startPositionOffset;
        minPosition = initialPosition - GetDirectionVector() * moveDistance / 2f;
        maxPosition = initialPosition + GetDirectionVector() * moveDistance / 2f;
        transform.position = initialPosition;

        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D missing on enemy: " + gameObject.name);
        }
        if (enemyCollider == null)
        {
            Debug.LogError("Collider2D missing on enemy: " + gameObject.name);
        }

        ConsonantLetterManager.instance?.RegisterEnemy(this); // Register with the manager
    }

    void OnDestroy()
    {
        ConsonantLetterManager.instance?.UnregisterEnemy(this); // Unregister when destroyed
    }

    void Update()
    {
        if (!isDead)
        {
            Move();
        }
    }

    void Move()
    {
        if (movePauseTimer > 0)
        {
            movePauseTimer -= Time.deltaTime;
            rb.velocity = Vector2.zero;
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

    Vector3 GetDirectionVector()
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
            playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y) + pushDirection * playerDescentSpeed;
        }

        enemyCollider.enabled = false;
        rb.velocity = Vector2.zero;

        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(ScoreManager.instance.scorePerKill);
        }

        Destroy(gameObject, deathDelay);
    }

    public void ResetEnemy()
    {
        isDead = false;
        transform.position = initialPosition;
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
        if (enemyCollider != null)
        {
            enemyCollider.enabled = true;
        }
        if (animator != null)
        {
            animator.SetBool("isDead", false);
        }
        Debug.Log(gameObject.name + " has been reset.");
        
    }
}