using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Damage Settings")]
    public float damageAmount = 20f;
    public string enemyTag = "Enemy";

    [Header("Respawn Settings")]
    public float respawnDelay = 2f;
    public bool isDead = false;
    private SpriteRenderer playerRenderer;
    private Collider2D playerCollider;
    private PlayerMovement2 playerMovementScript; // Reference to PlayerMovement

    [Header("UI References")]
    public Slider healthBarSlider;

    public static PlayerHealth instance; // Singleton instance
    public ConsonantEnemy enemy; // Reference to PlayerHealth
    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        playerRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
        playerMovementScript = GetComponent<PlayerMovement2>(); // Get PlayerMovement reference
        UpdateHealthUI();

        if (playerRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer missing on Player!");
        }
        if (playerCollider == null)
        {
            Debug.LogWarning("Collider2D missing on Player!");
        }
        if (playerMovementScript == null)
        {
            Debug.LogError("PlayerMovement script missing on Player!");
        }
    }

void OnCollisionEnter2D(Collision2D collision)
{
    if (isDead) return;

    if (collision.gameObject.CompareTag(enemyTag))
    {
        Collider2D enemyCollider = collision.collider;
        float playerBottom = playerCollider.bounds.min.y;
        float enemyTop = enemyCollider.bounds.max.y;

        // Check if the player's bottom is above the enemy's top by a small margin
        bool isJumpingOnEnemy = playerBottom > enemyTop - 0.1f;

        if (!isJumpingOnEnemy)
        {
            TakeDamage(damageAmount);
        }
    }
}


    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthUI();
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Player has died!");

        // Disable player controls and collision
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }
        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }

        // Hide the player
        if (playerRenderer != null)
        {
            playerRenderer.enabled = false;
        }

        // **Crucial Change: Restart the timer**
        IslandTimer.instance?.StartIslandTimer(); // Restart the timer when the player dies
        // Initiate respawn after a delay
        Invoke("Respawn", respawnDelay);
    }

    public void Respawn()
    {
        isDead = false;
        currentHealth = maxHealth;

        // Call the RespawnPlayer function in the PlayerMovement script
        if (playerMovementScript != null)
        {
            playerMovementScript.RespawnPlayer();
        }

        // Re-enable player controls and collision
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }
        if (playerCollider != null)
        {
            playerCollider.enabled = true;
        }

        // Show the player
        if (playerRenderer != null)
        {
            playerRenderer.enabled = true;
        }

        UpdateHealthUI();
        Debug.Log("Player has respawned!");

        // **Important: Reset other game elements here**
        ResetGameElements();
    }

    void UpdateHealthUI()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
        }
        else
        {
            Debug.LogError("Health Bar Slider not assigned in PlayerHealth script!");
        }
    }

    void ResetGameElements()
    {
        // Reset the score
        //ScoreManager.instance?.ResetScore(); // Assuming you add ResetScore() to ScoreManager

        // Reset the "died letters" (Assuming you have a manager for them)
        enemy?.ResetEnemy(); // Reset the enemy if needed
        // Add more reset logic here for other elements as needed
    }
}