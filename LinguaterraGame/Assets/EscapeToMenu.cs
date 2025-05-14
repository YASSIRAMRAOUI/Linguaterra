using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeToMenu : MonoBehaviour
{
    [Header("Name of the Menu Scene")]
    public string menuSceneName = "Menu"; // Change this to your menu scene's name

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC pressed - Returning to menu");
            SceneManager.LoadScene(menuSceneName);
        }
    }
}
