using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Assign the panel from the Inspector
    public Button resumeButton;
    public Button quitButton;

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitToMenu);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f; // Important to reset time before switching scenes
        SceneManager.LoadScene("Menu"); // Replace with your menu scene name
    }
     public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            Debug.Log("Game Paused");
            pauseMenuPanel.SetActive(true);

            resumeButton.onClick.AddListener(ResumeGame);
            quitButton.onClick.AddListener(QuitToMenu);
            }
        else
        {
            Time.timeScale = 1f;
            Debug.Log("Game Resumed");
            pauseMenuPanel.SetActive(true);
        }
    }
}
