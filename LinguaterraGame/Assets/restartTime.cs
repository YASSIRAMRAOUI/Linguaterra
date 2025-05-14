using UnityEngine;
using UnityEngine.SceneManagement;

public class restartTime : MonoBehaviour
{
    public float timeLimit = 240f ;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeLimit)
        {
            RestartGame();
        }
    }

    void RestartGame()
    {
        // Recharge la scène active
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
