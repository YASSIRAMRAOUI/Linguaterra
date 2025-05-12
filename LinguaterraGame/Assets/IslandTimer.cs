using UnityEngine;
using UnityEngine.UI;
using System;

public class IslandTimer : MonoBehaviour
{
    public float islandDurationSeconds = 120f;
    private float timeRemaining;
    private bool timerIsRunning = false;

    [Header("UI Display (Optional)")]
    public Text timerText;

    public static IslandTimer instance;

    public event Action OnIslandTimeExpired;

    void Awake()
    {
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
        // StartIslandTimer();  // REMOVED:  Don't start timer automatically
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
                OnIslandTimeExpired?.Invoke();
                PlayerHealth.instance?.Respawn();
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
        string timeString = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        timerText.text = timeString;
    }

    public float GetRemainingTime()
    {
        return timeRemaining;
    }

    public bool IsTimerRunning()
    {
        return timerIsRunning;
    }
}