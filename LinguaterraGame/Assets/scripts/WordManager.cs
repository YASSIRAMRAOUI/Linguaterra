using UnityEngine;
using TMPro;

public class WordManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI wordPanelText; // Référence au TextMeshPro qui affiche les lettres

    [Header("Mot à découvrir")]
    public string wordToFind = "A E U I O"; // Le mot à découvrir (peut être modifié dans l'éditeur)
    private string currentWord; // Mot avec lettres découvertes
    private int wordLength; // Longueur du mot à découvrir

    // Appelée au démarrage pour initialiser l'affichage
    void Start()
    {
        // Initialiser le mot avec des underscores
        wordLength = wordToFind.Length;
        currentWord = new string('_', wordLength); // Crée un mot comme "_ _ _ _ _ _"
        wordPanelText.text = currentWord; // Affiche ce mot au début
    }

    // Méthode pour mettre à jour le mot à découvrir
    public void UpdateWord(string letter)
    {
        bool letterFound = false;

        // Vérifie chaque lettre du mot à découvrir
        char[] wordArray = currentWord.ToCharArray();
        for (int i = 0; i < wordLength; i++)
        {
            if (wordToFind[i].ToString() == letter && wordArray[i] == '_')
            {
                wordArray[i] = letter[0]; // Remplace le "_" par la lettre correcte
                letterFound = true;
            }
        }

        // Si la lettre a été trouvée, mettre à jour l'affichage
        if (letterFound)
        {
            currentWord = new string(wordArray); // Met à jour le mot affiché
            wordPanelText.text = currentWord; // Afficher le mot mis à jour
        }

        // Vérifier si toutes les lettres ont été trouvées
        if (!currentWord.Contains("_"))
        {
            Debug.Log("Félicitations ! Vous avez trouvé le mot !");
            // Tu peux ici ajouter un message pour féliciter le joueur ou d'autres actions (ex: victoire)
        }
    }
}
