using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutoManager : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> tutorialPages;

    private int currentPage = 0;

    void Start()
    {
        player.SetActive(false);
        ShowPage(0);
    }

    public void ShowPage(int pageIndex)
    {
        for (int i = 0; i < tutorialPages.Count; i++)
        {
            tutorialPages[i].SetActive(i == pageIndex);
        }

        currentPage = pageIndex;
    }

    public void NextPage() // 👈 PUBLIC et SANS paramètre !
    {
        if (currentPage + 1 < tutorialPages.Count)
        {
            currentPage++;
            ShowPage(currentPage);
        }
        else
        {
            StartGame();
        }
    }

   public void StartGame()
{
    player.SetActive(true);

    // Active les mouvements
    PlayerMovement4 movement = player.GetComponent<PlayerMovement4>();
    if (movement != null)
    {
        movement.EnableMovement(); // 👈 active le déplacement
    }

    foreach (var page in tutorialPages)
    {
        page.SetActive(false);
    }
}

}
