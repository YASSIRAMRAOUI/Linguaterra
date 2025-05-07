using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI")]
    public TextMeshProUGUI scoreText;

    private int score = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddPoints(int points)
    {
        score += points;
        UpdateScoreUI();

        // Notifie tous les wizards de la scène
        WizardInteraction[] wizards = FindObjectsOfType<WizardInteraction>();
        foreach (WizardInteraction wizard in wizards)
        {
            wizard.OnScoreChanged();
            wizard.ForceDialogueUpdate(); // Double notification pour compatibilité
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public int GetScore()
    {
        return score;
    }
}