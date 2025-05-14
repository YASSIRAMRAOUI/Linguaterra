using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizUI4 : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI questionText;
    public Button buttonA;
    public Button buttonB;
    public Button buttonC;
    public Button validerButton;
    public Toggle toggleA;
    public Toggle toggleB;
    public Toggle toggleC;
    public AudioSource audioSource;

    private string lettreAttendue;
    private AudioClip sonCorrect;
    private AudioClip[] optionsSons;
    private AudioClip sonSelectionne;

    public WordManager4 wordManager;

    public void InitialiserQuiz(string lettre, AudioClip correctSon, AudioClip[] sonsOptions)
    {
        lettreAttendue = lettre;
        sonCorrect = correctSon;
        optionsSons = sonsOptions;
        sonSelectionne = null;

        questionText.text = $"Quel son correspond à la lettre {lettre} ?";

        // Configurer les boutons
        ConfigureButton(buttonA, 0);
        ConfigureButton(buttonB, 1);
        ConfigureButton(buttonC, 2);

        validerButton.onClick.RemoveAllListeners();
        validerButton.onClick.AddListener(ValiderReponse);
    }

    void ConfigureButton(Button button, int indexSon)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => {
            sonSelectionne = optionsSons[indexSon];
            PlaySound(sonSelectionne);
        });
    }

    void ValiderReponse()
    {
        if (sonSelectionne == null)
        {
            questionText.text = "Sélectionnez d'abord un son !";
            return;
        }

        if (sonSelectionne == sonCorrect)
        {
            questionText.text = $"Correct ! C'était bien le son de {lettreAttendue}";
            wordManager.UpdateWord(lettreAttendue); // Met à jour le mot avec la lettre trouvée
        }
        else
        {
            questionText.text = $"Incorrect. Essaies encore !";
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(clip);
        }
    }
}