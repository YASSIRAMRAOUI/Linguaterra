using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public GameObject keyObject; // L'objet cl� dans la sc�ne

    // M�thode pour afficher la cl�
    public void ShowKey()
    {
        keyObject.SetActive(true);
    }

    // M�thode pour cacher la cl�
    public void HideKey()
    {
        keyObject.SetActive(false);
    }
}
