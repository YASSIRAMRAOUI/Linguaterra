using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager3 : MonoBehaviour
{
    // Reference to player movement
    private PlayerMovement3 playerMovement;
    public GameObject panelToShow;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement3>();
        // Optional: hide the panel at the start
        if (panelToShow != null)
        {
            panelToShow.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered with: " + other.gameObject.name + " (Tag: " + other.gameObject.tag + ")");

        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (panelToShow != null)
            {
            Debug.Log("Player entered obstacle trigger!");
            panelToShow.SetActive(true); // Show the panel
            }
        }
    }
}