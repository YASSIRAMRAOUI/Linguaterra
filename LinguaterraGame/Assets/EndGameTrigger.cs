using UnityEngine;
using TMPro;
using System.Collections; // important pour IEnumerator

public class EndGameTrigger : MonoBehaviour
{
    public GameObject endGamePanel;
    public TMP_Text endGameMessageText;
    public TMP_Text finalScoreText;
    public AnimalPanelManager animalPanelManager; // pour récupérer le score

    private bool hasEnded = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasEnded && other.CompareTag("Player"))
        {
            hasEnded = true;
            ShowEndGamePanel();
        }
    }

    void ShowEndGamePanel()
    {
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(true);

            int score = animalPanelManager != null ? animalPanelManager.GetScore() : 0;

            if (endGameMessageText != null)
            {
                if (score == 0)
                    endGameMessageText.text = "It's okay hero, you can do it next time!";
                else if (score > 0 && score < 90)
                    endGameMessageText.text = "You don't know all the animals, but you did your best!";
                else
                    endGameMessageText.text = "Bravo little hero, you did it!";
            }

            if (finalScoreText != null)
                finalScoreText.text = "Score: " + score.ToString();

            StartCoroutine(HoldEndGamePanel());
        }
    }

    private IEnumerator HoldEndGamePanel()
    {
        Time.timeScale = 0f; // on stoppe le jeu
        yield return new WaitForSecondsRealtime(30f); // attendre 30 secondes en temps réel
        endGamePanel.SetActive(false);
        Time.timeScale = 1f; // relancer le jeu
    }
}
