using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public GameObject keyObject; // L'objet clé dans la scène

    // Méthode pour afficher la clé
    public void ShowKey()
    {
        keyObject.SetActive(true);
    }

    // Méthode pour cacher la clé
    public void HideKey()
    {
        keyObject.SetActive(false);
    }
}
