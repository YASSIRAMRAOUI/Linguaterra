using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyPickup : MonoBehaviour
{
    public WordManager wordManager; // Référence au WordManager
    public string nextSceneName = "ile2";    // Nom de la scène suivante à charger

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si le joueur touche la clé ET si toutes les lettres sont trouvées
        if (other.CompareTag("Player") && wordManager.AllLettersFound())
        {
            Debug.Log("Clé ramassée !");
            
            // Fais suivre la clé au joueur
            transform.SetParent(other.transform);
            transform.localPosition = Vector3.up * 1.5f;

            // Désactive le collider pour éviter de répéter l'action
            GetComponent<Collider>().enabled = false;

            // Charger la scène suivante
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Nom de la scène suivante non défini !");
        }
    }
}
