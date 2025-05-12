using UnityEngine;
using TMPro;

public class WizardInteraction : MonoBehaviour
{
    public Transform player;
    public float interactionDistance = 3f;
    public GameObject dialogueBubble;
    public TextMeshProUGUI dialogueText;
    public int targetScore = 200;

    private bool playerIsNear = false;

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < interactionDistance)
        {
            if (!playerIsNear)
            {
                playerIsNear = true;
                ShowDialogue();
            }
        }
        else
        {
            if (playerIsNear)
            {
                playerIsNear = false;
                HideDialogue();
            }
        }
    }

    // Méthode publique pour forcer la mise à jour du dialogue
    public void ForceDialogueUpdate()
    {
        ShowDialogue();
    }

    // Appelé quand le score change
    public void OnScoreChanged()
    {
        if (playerIsNear)
        {
            ShowDialogue();
        }
    }

    void ShowDialogue()
    {
        if (dialogueBubble == null) return;

        dialogueBubble.SetActive(true);

        bool targetReached = ScoreManageril2.Instance.GetScore() >= targetScore;

        dialogueText.text = targetReached
            ? "Well done! You’ve found all the letters! Here is the magic key to unlock the next island."
            : "Keep going! You still have more letters to find!";
    }

    void HideDialogue()
    {
        if (dialogueBubble != null)
            dialogueBubble.SetActive(false);
    }
}