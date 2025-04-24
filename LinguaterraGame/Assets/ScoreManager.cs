using UnityEngine;
using UnityEngine.UI; // For UI elements

public class ScoreManager : MonoBehaviour
{
    public int scorePerKill = 10; // Points awarded for each enemy kill
    private int currentScore = 0;

    [Header("UI Display")]
    public Text scoreText; // Assign a UI Text element in the Inspector

    public static ScoreManager instance; // Singleton instance for easy access

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
        // Optional: Don't destroy on scene load if you want the score to persist
        // DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        UpdateScoreText(); // Initialize the score display
    }

    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreText();
        Debug.Log("Score added! Current score: " + currentScore);
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore; // Or any format you like
        }
        else
        {
            Debug.LogWarning("Score Text not assigned in ScoreManager script!");
        }
    }
}