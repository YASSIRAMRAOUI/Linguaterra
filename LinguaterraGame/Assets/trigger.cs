using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    public GameObject victoryPanel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Victoire ! Le joueur a atteint la fin.");
            victoryPanel.SetActive(true);
        }
    }
}
