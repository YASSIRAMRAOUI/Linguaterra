using UnityEngine;
using UnityEngine.UI;

public class MergeQuizPanel : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject panel;           // The panel that shows the quiz
    public Text questionText;          // Text to display the question
    public Button answerA;             // Button A for answer choice
    public Button answerB;             // Button B for answer choice
    public Text feedbackText;          // Feedback text for correct/incorrect
    public Text scoreText;             // Text to display the player's score

    private string correctAnswer;      // Store the correct answer
    private int score = 0;             // Player's current score

    string[] consonants = { "m", "b", "t", "s", "d", "n" };
    string[] vowels = { "a", "o", "i", "e", "u" };

    void Start()
    {
        // Initially hide the panel and set the score text
        panel.SetActive(false);
        UpdateScoreDisplay();  // Ensure the score is displayed correctly at start

        // Attach button listeners for answers
        answerA.onClick.AddListener(() => CheckAnswer(answerA));
        answerB.onClick.AddListener(() => CheckAnswer(answerB));
    }

    public void ShowRandomQuestion()
    {
        panel.SetActive(true);  // Show the panel with the question

        // Pick random consonant and vowel
        string c = consonants[Random.Range(0, consonants.Length)];
        string v = vowels[Random.Range(0, vowels.Length)];
        correctAnswer = c + v;
        string wrongAnswer = v + c;

        // Set the question text
        questionText.text = $"Merge the consonant '{c}' with the vowel '{v}'";

        // Randomly assign correct and wrong answers to buttons
        if (Random.value < 0.5f)
        {
            answerA.GetComponentInChildren<Text>().text = correctAnswer;
            answerB.GetComponentInChildren<Text>().text = wrongAnswer;
        }
        else
        {
            answerA.GetComponentInChildren<Text>().text = wrongAnswer;
            answerB.GetComponentInChildren<Text>().text = correctAnswer;
        }

        // Clear any previous feedback
        feedbackText.text = "";
    }

    void CheckAnswer(Button clickedButton)
    {
        string chosen = clickedButton.GetComponentInChildren<Text>().text;

        if (chosen == correctAnswer)
        {
            // Correct answer
            feedbackText.text = "Correct!";
            feedbackText.color = Color.green;
            score += 10; // Increase the score for correct answer
            Debug.Log("✅ Correct!");
        }
        else
        {
            // Incorrect answer
            feedbackText.text = "Incorrect!";
            feedbackText.color = Color.red;
            Debug.Log("❌ Wrong! Try again.");
        }

        // Update the score on the UI
        UpdateScoreDisplay();

        // Optionally, hide the panel after a brief delay to let the player see feedback
        Invoke("HidePanel", 2f); // Hide the panel after 2 seconds
    }

    // Method to update the score text
    void UpdateScoreDisplay()
    {
        scoreText.text = "Score: " + score;
    }

    // Method to return the current score
    public int GetScore()
    {
        return score;
    }

    // Hide the panel after the feedback delay
    void HidePanel()
    {
        panel.SetActive(false);
    }
}
