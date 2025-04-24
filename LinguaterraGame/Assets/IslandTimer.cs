using UnityEngine;
using UnityEngine.UI; // If you want to display the timer on a UI Text element
using System; // For TimeSpan

public class IslandTimer : MonoBehaviour
{
    public float islandDurationSeconds = 120f; // Total time allowed for the island (in seconds)
    private float timeRemaining;
    private bool timerIsRunning = false;

    [Header("UI Display (Optional)")]
    public Text timerText; // Assign a UI Text element in the Inspector

    public static IslandTimer instance; // Singleton instance for easy access

    public event Action OnIslandTimeExpired; // Event that other scripts can subscribe to

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        StartIslandTimer();
    }

    void Update()
    {
        if (timerIsRunning)
        {
            timeRemaining -= Time.deltaTime;

            if (timerText != null)
            {
                UpdateTimerDisplay();
            }

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerIsRunning = false;
                Debug.Log("Island time has expired!");
                OnIslandTimeExpired?.Invoke(); // Fire the time expired event
                // You can add logic here for what happens when time runs out
                // (e.g., show a game over UI, transition to the next stage)
            }
        }
    }

    public void StartIslandTimer()
    {
        timeRemaining = islandDurationSeconds;
        timerIsRunning = true;
        if (timerText != null)
        {
            UpdateTimerDisplay();
        }
        Debug.Log("Island timer started. Duration: " + islandDurationSeconds + " seconds.");
    }

    public void StopIslandTimer()
    {
        timerIsRunning = false;
        Debug.Log("Island timer stopped.");
    }

    private void UpdateTimerDisplay()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeRemaining);
        // Format the time as minutes:seconds (e.g., 02:00)
        string timeString = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        timerText.text = timeString;
    }

    // Optional: Get the remaining time
    public float GetRemainingTime()
    {
        return timeRemaining;
    }

    // Optional: Check if the timer is running
    public bool IsTimerRunning()
    {
        return timerIsRunning;
    }
}