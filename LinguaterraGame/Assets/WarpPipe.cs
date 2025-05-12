using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpPipe : MonoBehaviour
{
    [Header("Warp Settings")]
    public int requiredScore = 100;
    public string nextIslandSceneName = "NextIslandScene";
    public string playerTag = "Player";

    [Header("Visual Settings")]
    public GameObject pipeVisuals; // Assign the GameObject that holds the pipe's visuals

    private bool canWarp = false;
    private Collider2D pipeCollider2D;
    private Collider pipeCollider;

    void Awake() //changed from start to awake
    {
        // Get the visual GameObject if assigned
        if (pipeVisuals == null)
        {
            pipeVisuals = gameObject; // If no separate visuals, assume this GameObject is it
        }

        // Get the collider components
        pipeCollider2D = GetComponent<Collider2D>();
        pipeCollider = GetComponent<Collider>();

        //check if the colliders are null
        if(pipeCollider2D == null && pipeCollider == null){
            Debug.LogError("WarpPipe: No collider found on " + gameObject.name + ".  Please add a 2D or 3D collider.");
        }

        //check if the playerTag is set
        if(string.IsNullOrEmpty(playerTag)){
             Debug.LogError("WarpPipe: playerTag is not set on " + gameObject.name + ".");
        }

        // Initially hide the visuals and disable the collider
        SetPipeVisibility(false);
        EnablePipeCollider(false);
    }

    void Update()
    {
        if (!canWarp)
        {
            if(ScoreManager.instance == null){
                Debug.LogWarning("WarpPipe: ScoreManager instance is null.  Cannot check score.");
                return; // IMPORTANT: Exit if no ScoreManager
            }
            if (ScoreManager.instance.GetCurrentScore() >= requiredScore)
            {
                // The required score has been reached
                canWarp = true;
                // Make the pipe visible and interactive
                SetPipeVisibility(true);
                EnablePipeCollider(true);
                Debug.Log("Warp Pipe is now active!  Score: " + ScoreManager.instance.GetCurrentScore());
                // You can add visual/audio cues here
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnTriggerEnterLogic(other.gameObject); //delegate to the common function
    }

    private void OnTriggerEnter(Collider other) // For 3D colliders
    {
        OnTriggerEnterLogic(other.gameObject); //delegate to the common function
    }

    private void OnTriggerEnterLogic(GameObject otherGameObject)
    {
         if (canWarp && otherGameObject.CompareTag(playerTag))
        {
            Debug.Log("Player: " + otherGameObject.name + " entered the Warp Pipe! Loading scene: " + nextIslandSceneName);
            SceneManager.LoadScene(nextIslandSceneName);
        }
        else if (canWarp)
        {
            Debug.Log("WarpPipe: OnTriggerEnter but with wrong tag: " + otherGameObject.tag + "  Expected tag: " + playerTag);
        }
        else
        {
            Debug.Log("WarpPipe: OnTriggerEnter but canWarp is false.  Score: " + (ScoreManager.instance != null ? ScoreManager.instance.GetCurrentScore() : -1)); //check for null instance
        }
    }

    private void SetPipeVisibility(bool visible)
    {
        if (pipeVisuals != null)
        {
            Renderer[] renderers = pipeVisuals.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = visible;
            }
        }
        else
        {
            Debug.LogWarning("WarpPipe: Pipe Visuals GameObject not assigned on " + gameObject.name);
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
