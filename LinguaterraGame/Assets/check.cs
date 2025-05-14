using UnityEngine;
using TMPro;

public class CheckAnimalWord : MonoBehaviour
{
    public Transform letterContainer; // 🎯 Le parent qui contient les lettres réordonnées
    public AnimalPanelManager panelManager;

    public void OnCheckClicked()
    {
        if (letterContainer == null)
        {
            Debug.LogWarning("Container de lettres non assigné.");
            return;
        }

        string playerAnswer = "";

        foreach (Transform letterObj in letterContainer)
        {
            TMP_Text letterText = letterObj.GetComponentInChildren<TMP_Text>();
            if (letterText != null)
            {
                playerAnswer += letterText.text.ToUpper();
            }
        }

        Debug.Log("Réponse du joueur depuis les boutons : " + playerAnswer);

        panelManager.CheckWord(playerAnswer);
    }
}
