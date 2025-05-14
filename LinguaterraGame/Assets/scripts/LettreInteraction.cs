using UnityEngine;

public class LettreInteraction : MonoBehaviour
{
    [Header("Configuration")]
    public string lettreName;
    public AudioClip sonCorrect;
    public AudioClip[] sonsLeurres;
    public GameObject quizUI;

    private bool hasAnsweredCorrectly = false;
    private bool hasScored = false; // Nouvelle variable pour suivre si les points ont déjà été ajoutés

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ShowQuiz();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HideQuiz();
        }
    }

    private void ShowQuiz()
    {
        if (sonCorrect == null || sonsLeurres.Length != 2)
        {
            Debug.LogError("Configuration audio manquante !", this);
            return;
        }

        AudioClip[] options = new AudioClip[3] { sonCorrect, sonsLeurres[0], sonsLeurres[1] };
        int correctIndex = 0;

        for (int i = 0; i < options.Length; i++)
        {
            int randomIndex = Random.Range(i, options.Length);
            (options[i], options[randomIndex]) = (options[randomIndex], options[i]);
            if (options[i] == sonCorrect) correctIndex = i;
        }

        QuizUI quizComponent = quizUI.GetComponent<QuizUI>();
        quizComponent.ResetUIComplet();
        quizComponent.InitialiserQuiz(lettreName, options, correctIndex, this);
        quizUI.SetActive(true);
    }

    private void HideQuiz()
    {
        if (quizUI != null)
        {
            quizUI.SetActive(false);
        }
    }

    public void SetAnsweredCorrectly()
    {
        hasAnsweredCorrectly = true;
    }

    public bool HasScored()
    {
        return hasScored;
    }

    public void SetScored()
    {
        hasScored = true;
    }
}
