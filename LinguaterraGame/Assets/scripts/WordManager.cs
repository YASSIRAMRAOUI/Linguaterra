using UnityEngine;
using TMPro;

public class WordManager : MonoBehaviour
{
    [Header("Éléments UI")]
    public TextMeshProUGUI wordPanelText; // Texte qui affiche le mot avec les trous
    public GameObject victoryPanel; // Panel à activer quand le mot est trouvé

    [Header("Configuration")]
    public string wordToFind = "A E U I O"; // Mot à deviner (avec espaces)

    private string hiddenWord; // Version cachée du mot
    private string displayedWord; // Mot affiché avec les lettres trouvées

    void Start()
    {
        InitializeWord();
    }

    void InitializeWord()
    {
        // Crée une version cachée sans espaces pour le traitement
        hiddenWord = wordToFind.Replace(" ", "");

        // Initialise l'affichage avec des underscores et espaces
        displayedWord = new string('_', hiddenWord.Length);
        UpdateDisplay();

        // Désactive le panel de victoire au départ
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    public void UpdateWord(string guessedLetter)
    {
        bool foundLetter = false;
        char[] wordArray = displayedWord.ToCharArray();

        // Compare chaque lettre sans tenir compte de la casse
        for (int i = 0; i < hiddenWord.Length; i++)
        {
            if (char.ToUpper(hiddenWord[i]) == char.ToUpper(guessedLetter[0])
                && wordArray[i] == '_')
            {
                wordArray[i] = wordToFind[i * 2]; // Prend la casse originale
                foundLetter = true;
            }
        }

        if (foundLetter)
        {
            displayedWord = new string(wordArray);
            UpdateDisplay();

            // Vérifie si le mot est complet
            if (!displayedWord.Contains("_"))
            {
                ShowVictory();
            }
        }
    }

    void UpdateDisplay()
    {
        // Ajoute des espaces entre les caractères pour l'affichage
        wordPanelText.text = string.Join(" ", displayedWord.ToCharArray());
    }

    void ShowVictory()
    {
        Debug.Log("Mot complet trouvé !");

        if (victoryPanel != null)
            victoryPanel.SetActive(true);
    }

    // Optionnel : Pour réinitialiser le jeu
    public void ResetGame()
    {
        InitializeWord();
    }
}