using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizUI3 : MonoBehaviour
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

    public WordManager3 wordManager;

    public void InitialiserQuiz(string lettre, AudioClip correctSon, AudioClip[] sonsOptions)
    {
        lettreAttendue = lettre;
        sonCorrect = correctSon;
        optionsSons = sonsOptions;
        sonSelectionne = null;

        questionText.text = $" Which sound matches the letter {lettre} ?";

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
            questionText.text = "Please select a sound first !";
            return;
        }

        if (sonSelectionne == sonCorrect)
        {
            questionText.text = $"Correct! That was indeed the sound of {lettreAttendue}";
            wordManager.UpdateWord(lettreAttendue); // Met à jour le mot avec la lettre trouvée
        }
        else
        {
            questionText.text = $"Incorrect. Try again !";
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