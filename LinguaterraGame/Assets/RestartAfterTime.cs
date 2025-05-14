using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RestartAfterTime : MonoBehaviour
{
    public float timeLimit = 240f;
    private float timer = 0f;

    public TextMeshProUGUI timecount; // <-- Ton texte s'appelle timecount

    void Update()
{
    timer += Time.unscaledDeltaTime;  // <--- changement ici
    float timeLeft = Mathf.Max(0, timeLimit - timer);

    UpdateTimerDisplay(timeLeft);

    if (timer >= timeLimit)
    {
        RestartGame();
    }
}


    void UpdateTimerDisplay(float timeLeft)
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60f);
        int seconds = Mathf.FloorToInt(timeLeft % 60f);

        if (timecount != null)
        {
            timecount.text = $"Temps restant : {minutes:00}:{seconds:00}";
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
