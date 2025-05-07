using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class QuizUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI questionText;
    public Button[] listenButtons;
    public Toggle[] answerToggles;
    public GameObject[] arrows;
    public Button validateButton;
    public AudioSource audioSource;
    public WordManager wordManager;

    [Header("Colors")]
    public Color defaultColor = Color.white;
    public Color selectedColor = Color.yellow;
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;

    private AudioClip[] currentOptions;
    private int correctIndex;
    private int selectedIndex = -1;
    private string currentLetter;
    private LettreInteraction currentLettreInteraction;

    void Start()
    {
        InitializeToggles();

        for (int i = 0; i < listenButtons.Length; i++)
        {
            int index = i;
            listenButtons[i].onClick.AddListener(() => PlaySound(index));
            answerToggles[i].onValueChanged.AddListener((isOn) => OnToggleChanged(index, isOn));
        }
        validateButton.onClick.AddListener(ValidateAnswer);

        ResetUIComplet();
    }

    private void InitializeToggles()
    {
        foreach (Toggle toggle in answerToggles)
        {
            if (toggle != null)
            {
                toggle.isOn = false;
                if (toggle.graphic != null)
                    toggle.graphic.gameObject.SetActive(false);
                if (toggle.image != null)
                    toggle.image.color = defaultColor;
            }
        }
    }

    public void InitialiserQuiz(string lettre, AudioClip[] options, int indexCorrect, LettreInteraction interaction)
    {
        currentLettreInteraction = interaction;
        currentLetter = lettre;
        currentOptions = options;
        correctIndex = indexCorrect;
        selectedIndex = -1;
        questionText.text = $"What sound matches the letter {lettre} ?";

        ForceVisualReset();
        SetInteractable(true); // Réactive les interactions quand un nouveau quiz commence
    }

    public void ResetUIComplet()
    {
        for (int i = 0; i < answerToggles.Length; i++)
        {
            if (answerToggles[i] != null)
            {
                answerToggles[i].isOn = false;
                if (answerToggles[i].image != null)
                    answerToggles[i].image.color = defaultColor;

                if (answerToggles[i].graphic != null)
                {
                    answerToggles[i].graphic.gameObject.SetActive(false);
                    answerToggles[i].graphic.color = defaultColor;
                }
            }

            if (i < arrows.Length && arrows[i] != null)
                arrows[i].SetActive(false);
        }

        selectedIndex = -1;
        Canvas.ForceUpdateCanvases();
    }

    private void ForceVisualReset()
    {
        foreach (Toggle toggle in answerToggles)
        {
            if (toggle != null)
            {
                toggle.isOn = false;
                if (toggle.image != null)
                    toggle.image.color = defaultColor;
            }
        }

        foreach (GameObject arrow in arrows)
        {
            if (arrow != null)
                arrow.SetActive(false);
        }
    }

    private void PlaySound(int index)
    {
        if (currentOptions != null && index >= 0 && index < currentOptions.Length)
        {
            audioSource.PlayOneShot(currentOptions[index]);
        }
    }

    private void OnToggleChanged(int index, bool isOn)
    {
        if (isOn)
        {
            selectedIndex = index;
            UpdateVisuals();
        }
    }

    private void UpdateVisuals()
    {
        for (int i = 0; i < answerToggles.Length; i++)
        {
            bool isSelected = (i == selectedIndex);

            if (answerToggles[i] != null && answerToggles[i].image != null)
                answerToggles[i].image.color = isSelected ? selectedColor : defaultColor;

            if (i < arrows.Length && arrows[i] != null)
                arrows[i].SetActive(isSelected);

            if (answerToggles[i] != null && answerToggles[i].graphic != null)
                answerToggles[i].graphic.gameObject.SetActive(isSelected);
        }
    }

    private void ValidateAnswer()
    {
        if (selectedIndex == -1) return;

        bool isCorrect = (selectedIndex == correctIndex);

        if (answerToggles[selectedIndex] != null && answerToggles[selectedIndex].image != null)
            answerToggles[selectedIndex].image.color = isCorrect ? correctColor : wrongColor;

        if (isCorrect)
        {
            questionText.text = "Well done! That was the correct sound";

            if (wordManager != null && !currentLettreInteraction.HasScored())
            {
                ScoreManager.Instance.AddPoints(40);
                wordManager.UpdateWord(currentLetter);
                currentLettreInteraction.SetScored(); // Marquer que les points ont été ajoutés
            }

            SetInteractable(false);
            StartCoroutine(CompleteCorrectAnswerAfterDelay(2f));
        }
        else
        {
            questionText.text = "Incorrect! Try again";
        }
    }

    private IEnumerator CompleteCorrectAnswerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (currentLettreInteraction != null)
        {
            currentLettreInteraction.SetAnsweredCorrectly();
        }

        gameObject.SetActive(false);
    }

    private void SetInteractable(bool state)
    {
        foreach (var button in listenButtons) button.interactable = state;
        foreach (var toggle in answerToggles) toggle.interactable = state;
        validateButton.interactable = state;
    }
}
