using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections;

public class AnimalPanelManager : MonoBehaviour
{
    public AnimalData[] animals;
    public Image animalImage;
    public Text assembledText;
    public TMP_Text feedbackText;
    public TMP_Text scoreText;

    public AudioSource backgroundMusicSource;
    public AudioClip defaultBackgroundMusic;
    public GameObject currentPanel;
    public AudioClip currentAnimalSound;
    private string currentWord;
    private string assembledWord;
    public Transform letterContainer;
    public GameObject letterButtonPrefab;

    private AudioSource audioSource;

    private int currentAnimalIndex;
    private int attempts;
    private int score;
    private bool challengeStarted = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        foreach (Transform child in transform)
            child.gameObject.SetActive(false);

        if (feedbackText != null)
        {
            feedbackText.text = "";
            feedbackText.gameObject.SetActive(false);
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: 0";
        }

        score = 0;
    }

    public void StartAnimalChallenge(int animalIndex)
{
    if (challengeStarted) return;
    challengeStarted = true;

    if (animalIndex < 0 || animalIndex >= animals.Length)
    {
        Debug.LogError("Index de l'animal invalide");
        return;
    }

    currentAnimalIndex = animalIndex;
    AnimalData animal = animals[animalIndex];
    currentWord = animal.animalName.ToUpper();
    currentPanel = animal.associatedPanel;

    // METTRE EN PAUSE la musique de fond ici
    if (backgroundMusicSource != null && backgroundMusicSource.isPlaying)
    {
        backgroundMusicSource.Pause();
    }

    if (animalImage != null && animal.animalImage != null)
    {
        animalImage.sprite = animal.animalImage;
    }

    currentAnimalSound = animal.animalSound;

    currentPanel.SetActive(true);
    GenerateLetters();

    assembledWord = "";
    if (assembledText != null)
        assembledText.text = "";

    if (feedbackText != null)
    {
        feedbackText.text = "";
        feedbackText.gameObject.SetActive(false);
    }

    Time.timeScale = 0f;
    attempts = 0;

    if (animalImage != null)
    {
        Button imageButton = animalImage.GetComponent<Button>();
        if (imageButton == null)
            imageButton = animalImage.gameObject.AddComponent<Button>();

        imageButton.onClick.RemoveAllListeners();
        imageButton.onClick.AddListener(PlayCurrentAnimalSound);
    }
}


    public void PlayCurrentAnimalSound()
    {
        if (currentAnimalIndex < 0 || currentAnimalIndex >= animals.Length) return;

        AudioClip clip = animals[currentAnimalIndex].animalSound;
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }

    public void EndAnimalChallenge()
    {
        if (backgroundMusicSource != null && defaultBackgroundMusic != null)
        {
            backgroundMusicSource.clip = defaultBackgroundMusic;
            backgroundMusicSource.Play();
        }
       


        if (currentPanel != null)
        {
            currentPanel.SetActive(false);
        }

        Time.timeScale = 1f;
        challengeStarted = false;
    }

    public void CheckWord(string wordToCheck)
    {
        attempts++;

        if (wordToCheck.ToUpper() == currentWord)
        {
            int earnedPoints = 0;
            if (attempts == 1)
                earnedPoints = 30;
            else if (attempts == 2)
                earnedPoints = 20;
            else if (attempts == 3)
                earnedPoints = 10;
            
            score += earnedPoints;
            UpdateScoreUI();

            StartCoroutine(ShowFeedbackAndClose(true, false));
        }
        else
        {
            if (attempts >= 3)
            {
                score += 0;
                UpdateScoreUI();
                StartCoroutine(ShowFeedbackAndClose(false, true)); // montrer la réponse correcte
            }
            else
            {
                StartCoroutine(ShowFeedbackAndClose(false, false));
            }
        }
    }

    IEnumerator ShowFeedbackAndClose(bool success, bool showCorrectAnswer)
    {
        if (feedbackText != null)
        {
            feedbackText.gameObject.SetActive(true);
            if (success)
            {
                feedbackText.color = Color.green;
                feedbackText.text = "Amazing!";
            }
            else
            {
                feedbackText.color = Color.red;
                if (showCorrectAnswer)
                    feedbackText.text = "The answer is:" + currentWord;
                else
                    feedbackText.text = "Retry!";
            }
        }

        yield return new WaitForSecondsRealtime(2.5f);

        if (success || showCorrectAnswer)
        {
            EndAnimalChallenge();
            ClearLetters();
        }
        else
        {
            ClearAssembledWord();

            if (feedbackText != null)
            {
                feedbackText.text = "";
                feedbackText.gameObject.SetActive(false);
            }
        }
    }

    void GenerateLetters()
    {
        ClearLetters();
        assembledWord = "";
        if (assembledText != null)
            assembledText.text = "";

        char[] letters = currentWord.ToCharArray();
        System.Random rnd = new System.Random();
        letters = letters.OrderBy(c => rnd.Next()).ToArray();

        foreach (char c in letters)
        {
            GameObject letterObj = Instantiate(letterButtonPrefab, letterContainer);

            TMP_Text tmpText = letterObj.GetComponentInChildren<TMP_Text>();
            if (tmpText != null)
                tmpText.text = c.ToString();

            if (letterObj.GetComponent<CanvasGroup>() == null)
                letterObj.AddComponent<CanvasGroup>();

            DraggableLetter draggableLetter = letterObj.GetComponent<DraggableLetter>();
            if (draggableLetter == null)
                draggableLetter = letterObj.AddComponent<DraggableLetter>();

            Button button = letterObj.GetComponent<Button>();
            if (button != null)
                button.onClick.AddListener(() => OnLetterClicked(c.ToString()));
        }
    }

    void OnLetterClicked(string letter)
    {
        assembledWord += letter;

        if (assembledText != null)
            assembledText.text = assembledWord;

        if (assembledWord.Length == currentWord.Length)
            CheckWord(assembledWord);
    }

    void ClearLetters()
    {
        foreach (Transform child in letterContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void ClearAssembledWord()
    {
        assembledWord = "";
        if (assembledText != null)
            assembledText.text = "";
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();
    }

    void Awake()
    {
        if (animalImage == null) Debug.LogWarning("animalImage n'est pas assigné.");
        if (letterButtonPrefab == null) Debug.LogWarning("letterButtonPrefab n'est pas assigné.");
        if (letterContainer == null) Debug.LogWarning("letterContainer n'est pas assigné.");
        if (feedbackText == null) Debug.LogWarning("feedbackText n'est pas assigné.");
        if (scoreText == null) Debug.LogWarning("scoreText n'est pas assigné.");
    }
    public int GetScore()
{
    return score;
}

}
