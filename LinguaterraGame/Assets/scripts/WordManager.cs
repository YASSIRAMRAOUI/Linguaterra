using UnityEngine;
using TMPro;

public class WordManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI wordPanelText; // R�f�rence au TextMeshPro qui affiche les lettres

    [Header("Mot � d�couvrir")]
    public string wordToFind = "A E U I O"; // Le mot � d�couvrir (peut �tre modifi� dans l'�diteur)
    private string currentWord; // Mot avec lettres d�couvertes
    private int wordLength; // Longueur du mot � d�couvrir

    // Appel�e au d�marrage pour initialiser l'affichage
    void Start()
    {
        // Initialiser le mot avec des underscores
        wordLength = wordToFind.Length;
        currentWord = new string('_', wordLength); // Cr�e un mot comme "_ _ _ _ _ _"
        wordPanelText.text = currentWord; // Affiche ce mot au d�but
    }

    // M�thode pour mettre � jour le mot � d�couvrir
    public void UpdateWord(string letter)
    {
        bool letterFound = false;

        // V�rifie chaque lettre du mot � d�couvrir
        char[] wordArray = currentWord.ToCharArray();
        for (int i = 0; i < wordLength; i++)
        {
            if (wordToFind[i].ToString() == letter && wordArray[i] == '_')
            {
                wordArray[i] = letter[0]; // Remplace le "_" par la lettre correcte
                letterFound = true;
            }
        }

        // Si la lettre a �t� trouv�e, mettre � jour l'affichage
        if (letterFound)
        {
            currentWord = new string(wordArray); // Met � jour le mot affich�
            wordPanelText.text = currentWord; // Afficher le mot mis � jour
        }

        // V�rifier si toutes les lettres ont �t� trouv�es
        if (!currentWord.Contains("_"))
        {
            Debug.Log("F�licitations ! Vous avez trouv� le mot !");
            // Tu peux ici ajouter un message pour f�liciter le joueur ou d'autres actions (ex: victoire)
        }
    }
}
