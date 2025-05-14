using UnityEngine;
using UnityEngine.UI;

public class MergeQuizPanel : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject panel;
    public Text questionText;
    public Button answerA;
    public Button answerB;
    public Text feedbackText; // Feedback text for correct/incorrect
    public Text scoreText;    // Score text to display the player's score

    private string correctAnswer;
    private int score = 0; // Player's score

    string[] consonants = { "m", "b", "t", "s", "d", "n" };
    string[] vowels = { "a", "o", "i", "e", "u" };

    void Start()
    {
        panel.SetActive(false);
        scoreText.text = "Score: " + score; // Display the initial score

        answerA.onClick.AddListener(() => CheckAnswer(answerA));
        answerB.onClick.AddListener(() => CheckAnswer(answerB));
    }

    public void ShowRandomQuestion()
    {
        panel.SetActive(true);

        // Pick random consonant and vowel
        string c = consonants[Random.Range(0, consonants.Length)];
        string v = vowels[Random.Range(0, vowels.Length)];
        correctAnswer = c + v;
        string wrongAnswer = v + c;

        questionText.text = $"Merge the consonant '{c}' with the vowel '{v}'";

        // Randomly assign correct and wrong answer to buttons
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

        feedbackText.text = ""; // Clear any previous feedback text
    }

    void CheckAnswer(Button clickedButton)
    {
        string chosen = clickedButton.GetComponentInChildren<Text>().text;

        if (chosen == correctAnswer)
        {
            feedbackText.text = "Correct!"; // Display feedback text
            feedbackText.color = Color.green; // Optional: green for correct
            score += 10; // Add score for correct answer
            Debug.Log("✅ Correct!");
        }
        else
        {
            feedbackText.text = "Incorrect!"; // Display feedback text
            feedbackText.color = Color.red; // Optional: red for incorrect
            Debug.Log("❌ Wrong! Try again.");
        }

        // Update score display
        scoreText.text = "Score: " + score;

        // Optionally, hide the panel after a brief delay
        Invoke("HidePanel", 2f); // Hide after 2 seconds
    }

    // Hide the panel after the feedback
    void HidePanel()
    {
        panel.SetActive(false);
    }
}
