using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public WordManager wordManager; // R�f�rence au WordManager

    private void OnTriggerEnter(Collider other)
    {
        // V�rifie si le joueur touche la cl� ET si toutes les lettres sont trouv�es
        if (other.CompareTag("Player") && wordManager.allLettersFound)
        {
            Debug.Log("Cl� ramass�e !");
            // Fais suivre la cl� au joueur (ex: la mettre comme enfant du joueur)
            transform.SetParent(other.transform);
            transform.localPosition = Vector3.up * 1.5f; // Position au-dessus du joueur

            // D�sactive le collider pour �viter de r�p�ter l'action
            GetComponent<Collider>().enabled = false;
        }
    }
}