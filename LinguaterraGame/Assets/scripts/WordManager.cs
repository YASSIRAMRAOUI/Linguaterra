using UnityEngine;
using TMPro;

public class WordManager : MonoBehaviour
{
    [Header("Éléments UI")]
    public TextMeshProUGUI wordPanelText;
    public GameObject victoryPanel;

    [Header("Configuration")]
    public string wordToFind = "A E U I O";

    private string hiddenWord;
    private string displayedWord;
    public bool allLettersFound { get; private set; } = false;
    private int correctLettersCount = 0;

    void Start()
    {
        InitializeWord();
    }

    void InitializeWord()
    {
        hiddenWord = wordToFind.Replace(" ", "");
        Debug.Log("hiddenWord: " + hiddenWord);

        displayedWord = new string('_', hiddenWord.Length);
        UpdateDisplay();

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        allLettersFound = false;
        correctLettersCount = 0;
    }

    public void UpdateWord(string guessedLetter)
    {
        Debug.Log($"UpdateWord called with: {guessedLetter}");
        Debug.Log($"Current progress: {correctLettersCount}/{hiddenWord.Length}");

        bool foundLetter = false;
        char[] wordArray = displayedWord.ToCharArray();

        for (int i = 0; i < hiddenWord.Length; i++)
        {
            if (char.ToUpper(hiddenWord[i]) == char.ToUpper(guessedLetter[0]) && wordArray[i] == '_')
            {
                wordArray[i] = wordToFind[i * 2]; // Prend la casse originale
                foundLetter = true;
                correctLettersCount++;
                Debug.Log($"Found '{guessedLetter}'. Count: {correctLettersCount}/{hiddenWord.Length}");
            }
        }

        if (foundLetter)
        {
            displayedWord = new string(wordArray);
            UpdateDisplay();

            if (correctLettersCount >= hiddenWord.Length)
            {
                ShowVictory();
            }
        }
    }

    void UpdateDisplay()
    {
        wordPanelText.text = string.Join(" ", displayedWord.ToCharArray());
        Debug.Log("Displayed: " + wordPanelText.text);
    }

    void ShowVictory()
    {
        allLettersFound = true;
        Debug.Log("VICTORY! allLettersFound = " + allLettersFound);

        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        // Notifie WizardInteraction que le mot est complet
        WizardInteraction wizard = FindObjectOfType<WizardInteraction>();
        if (wizard != null)
        {
            wizard.ForceDialogueUpdate();
        }
    }

    public void ResetGame()
    {
        InitializeWord();
    }
}
