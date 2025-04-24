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
    private bool isDead = false;
    private SpriteRenderer playerRenderer;
    private Collider2D playerCollider;
    private PlayerMovement playerMovementScript; // Reference to PlayerMovement

    [Header("UI References")]
    public Slider healthBarSlider;

    void Start()
    {
        currentHealth = maxHealth;
        playerRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
        playerMovementScript = GetComponent<PlayerMovement>(); // Get PlayerMovement reference
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
            TakeDamage(damageAmount);
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

        // Initiate respawn after a delay
        Invoke("Respawn", respawnDelay);
    }

    void Respawn()
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
}