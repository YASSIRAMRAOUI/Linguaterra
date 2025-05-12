using UnityEngine;
using UnityEngine.UI;
using System.Collections; // For coroutines

public class InstructionController : MonoBehaviour
{
    public GameObject instructionsPanel;
    public Button startButton;
    public float fadeOutDuration = 0.5f;

    private Animator animator;
    private bool instructionsActive = true;

    void Start()
    {
        animator = instructionsPanel.GetComponent<Animator>();
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
        ShowInstructions();
        // IslandTimer.instance?.StopIslandTimer();  // REMOVED: Don't stop it here
    }

    void Update()
    {
        if (instructionsActive && Input.anyKeyDown)
        {
            StartGame();
        }
    }

    void ShowInstructions()
    {
        if (instructionsPanel != null && animator != null)
        {
            instructionsPanel.SetActive(true);
            animator.SetTrigger("ShowInstructions");
        }
    }

    void HideInstructions()
    {
        if (animator != null)
        {
            animator.SetTrigger("HideInstructions");
        }
        StartCoroutine(DeactivatePanelAfterFade());
        instructionsActive = false;
        // IslandTimer.instance?.StartIslandTimer(); // MOVED: Start timer here!
    }

    void StartGame()
    {
        if (instructionsActive)
        {
            HideInstructions();
            IslandTimer.instance?.StartIslandTimer(); // START TIMER HERE
        }
    }

    IEnumerator DeactivatePanelAfterFade()
    {
        yield return new WaitForSeconds(fadeOutDuration);
        instructionsPanel.SetActive(false);
    }
}