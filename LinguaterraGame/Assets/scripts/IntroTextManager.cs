using UnityEngine;
using UnityEngine.UI;
using TMPro; // Ajoutez cette ligne

public class IntroTextManager : MonoBehaviour
{
    public TMP_Text dialogueText; // Changez Text en TMP_Text
    public Button nextButton;

    [TextArea(3, 10)]
    public string[] messages;

    private int currentIndex = 0;

    void Start()
    {
        if (messages.Length > 0)
        {
            dialogueText.text = messages[currentIndex];
        }

        nextButton.onClick.AddListener(ShowNextMessage);
    }

    void ShowNextMessage()
    {
        currentIndex++;
        if (currentIndex < messages.Length)
        {
            dialogueText.text = messages[currentIndex];
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}