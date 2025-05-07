using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public WordManager wordManager; // Référence au WordManager

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si le joueur touche la clé ET si toutes les lettres sont trouvées
        if (other.CompareTag("Player") && wordManager.allLettersFound)
        {
            Debug.Log("Clé ramassée !");
            // Fais suivre la clé au joueur (ex: la mettre comme enfant du joueur)
            transform.SetParent(other.transform);
            transform.localPosition = Vector3.up * 1.5f; // Position au-dessus du joueur

            // Désactive le collider pour éviter de répéter l'action
            GetComponent<Collider>().enabled = false;
        }
    }
}