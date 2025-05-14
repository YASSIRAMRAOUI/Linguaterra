using UnityEngine;

public class EauCollision3 : MonoBehaviour
{
    [Header("UI Panel References")]
    public GameObject panelToShow;       // Assign the quiz panel in Inspector
    public MergeQuizPanel quizPanel;     // Assign the script that controls the quiz

    [Header("Player Movement")]
    private PlayerMovement3 playerMovement;

    [Header("Respawn Position")]
    public Vector3 respawnPosition = new Vector3(-3.5f, 0.5f, -25.2f); // Customize as needed

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement3>();
        if (playerMovement == null)
            Debug.LogWarning("PlayerMovement3 script is not found on this GameObject.");

        if (panelToShow == null)
            Debug.LogWarning("PanelToShow is not assigned in the Inspector.");
        else
            panelToShow.SetActive(false); // Hide panel at start

        if (quizPanel == null)
            Debug.LogWarning("QuizPanel script reference is missing.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Eau"))
        {
            Debug.Log("Entered water.");

            if (playerMovement != null)
                playerMovement.RespawnPlayer(respawnPosition);
        }

        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Entered obstacle.");

            if (panelToShow != null)
                panelToShow.SetActive(true);

            if (quizPanel != null)
                quizPanel.ShowRandomQuestion();
        }
    }
}
