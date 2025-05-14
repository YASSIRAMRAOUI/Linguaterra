using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    public Toggle toggleA; // Le premier toggle
    public Toggle toggleB; // Le deuxième toggle
    public Toggle toggleC; // Le troisième toggle

    public GameObject arrowA; // Flèche à côté de toggleA
    public GameObject arrowB; // Flèche à côté de toggleB
    public GameObject arrowC; // Flèche à côté de toggleC

    // Start est appelé avant la première image
    void Start()
    {
        // D'abord forcer tous les toggles à être éteints
        toggleA.isOn = false;
        toggleB.isOn = false;
        toggleC.isOn = false;

        // Puis désactiver toutes les flèches
        DeselectArrows();

        // Ensuite seulement, écouter les changements
        toggleA.onValueChanged.AddListener((isOn) => OnToggleChanged(toggleA, isOn, arrowA));
        toggleB.onValueChanged.AddListener((isOn) => OnToggleChanged(toggleB, isOn, arrowB));
        toggleC.onValueChanged.AddListener((isOn) => OnToggleChanged(toggleC, isOn, arrowC));
    }

    // Désélectionner toutes les flèches (cacher les flèches)
    void DeselectArrows()
    {
        arrowA.SetActive(false);
        arrowB.SetActive(false);
        arrowC.SetActive(false);
    }

    // Gérer l'état du toggle et afficher la flèche correspondante
    void OnToggleChanged(Toggle toggle, bool isOn, GameObject arrow)
    {
        if (isOn)
        {
            // Si le toggle est activé, afficher la flèche à côté
            arrow.SetActive(true);
        }
        else
        {
            // Si le toggle est désactivé, cacher la flèche
            arrow.SetActive(false);
        }
    }
}
