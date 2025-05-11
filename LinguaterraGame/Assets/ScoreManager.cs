using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int scorePerKill = 10;
    private int currentScore = 0;

    [Header("UI Display")]
    public Text scoreText;

    public static ScoreManager instance;

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
        UpdateScoreText();
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
            scoreText.text = "Score: " + currentScore;
        }
        else
        {
            Debug.LogWarning("Score Text not assigned in ScoreManager script!");
        }
    }

    // **New: Reset Score Function**
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreText();
        Debug.Log("Score reset!");
    }
}