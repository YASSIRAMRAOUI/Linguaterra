using UnityEngine;
using UnityEngine.SceneManagement;

public class Pipe3 : MonoBehaviour
{
    [Header("Warp Settings")]
    public int requiredScore = 100;              // The score required to warp
    public string nextIslandSceneName = "ile4"; // The scene to load after warping
    public string playerTag = "Player";          // Tag for the player

    [Header("Visual Settings")]
    public GameObject pipeVisuals;               // GameObject that holds the pipe's visuals

    private bool canWarp = false;                // Whether the player can warp
    private Collider2D pipeCollider2D;           // The 2D collider on the pipe
    private Collider pipeCollider;               // The 3D collider on the pipe

    public MergeQuizPanel quizPanel;             // Reference to the MergeQuizPanel script

    void Awake()
    {
        // Get the visual GameObject if not assigned
        if (pipeVisuals == null)
        {
            pipeVisuals = gameObject; // If no separate visuals, assume this GameObject is it
        }

        // Get the collider components
        pipeCollider2D = GetComponent<Collider2D>();
        pipeCollider = GetComponent<Collider>();

        // Check if the colliders are null
        if (pipeCollider2D == null && pipeCollider == null)
        {
            Debug.LogError("WarpPipe: No collider found on " + gameObject.name + ". Please add a 2D or 3D collider.");
        }

        // Ensure quiz panel is set

        // Initially hide the visuals and disable the collider
        SetPipeVisibility(false);
        EnablePipeCollider(false);
    }

    void Update()
    {
        if (!canWarp)
        {
            // Ensure the MergeQuizPanel instance is available
           // if (quizPanel == null)
            //{
              //  Debug.LogWarning("Pipe3: MergeQuizPanel is missing, cannot check score.");
                //return; // Exit if no quiz panel
            //}

            // Check if the score has reached the required threshold to enable the warp

                // The required score has been reached
                canWarp = true;
                SetPipeVisibility(true);
                EnablePipeCollider(true);
                //Debug.Log("Warp Pipe is now active! Score: " + quizPanel.GetScore());
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Call common function for handling triggers with 2D colliders
        OnTriggerEnterLogic(other.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Call common function for handling triggers with 3D colliders
        OnTriggerEnterLogic(other.gameObject);
    }

    private void OnTriggerEnterLogic(GameObject otherGameObject)
    {
        // When the player enters the warp pipe
        if (canWarp && otherGameObject.CompareTag(playerTag))
        {
            Debug.Log("Player: " + otherGameObject.name + " entered the Warp Pipe! Loading scene: " + nextIslandSceneName);

            SceneManager.LoadScene(nextIslandSceneName); // Load the next scene (island)
        }
        else if (canWarp)
        {
            Debug.Log("WarpPipe: OnTriggerEnter but with wrong tag: " + otherGameObject.tag + "  Expected tag: " + playerTag);
        }
        else
        {
            Debug.Log("WarpPipe: OnTriggerEnter but canWarp is false. Score: " + (quizPanel != null ? quizPanel.GetScore() : -1)); // Handle null quizPanel case
        }
    }

    private void SetPipeVisibility(bool visible)
    {
            Renderer[] renderers = pipeVisuals.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = visible;
            }
    }

    private void EnablePipeCollider(bool enable)
    {
        if (pipeCollider2D != null)
        {
            pipeCollider2D.enabled = enable;
        }
        if (pipeCollider != null)
        {
            pipeCollider.enabled = enable;
        }
    }
}
