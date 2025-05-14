using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    public Toggle toggleA; // Le premier toggle
    public Toggle toggleB; // Le deuxi�me toggle
    public Toggle toggleC; // Le troisi�me toggle

    public GameObject arrowA; // Fl�che � c�t� de toggleA
    public GameObject arrowB; // Fl�che � c�t� de toggleB
    public GameObject arrowC; // Fl�che � c�t� de toggleC

    // Start est appel� avant la premi�re image
    void Start()
    {
        // D'abord forcer tous les toggles � �tre �teints
        toggleA.isOn = false;
        toggleB.isOn = false;
        toggleC.isOn = false;

        // Puis d�sactiver toutes les fl�ches
        DeselectArrows();

        // Ensuite seulement, �couter les changements
        toggleA.onValueChanged.AddListener((isOn) => OnToggleChanged(toggleA, isOn, arrowA));
        toggleB.onValueChanged.AddListener((isOn) => OnToggleChanged(toggleB, isOn, arrowB));
        toggleC.onValueChanged.AddListener((isOn) => OnToggleChanged(toggleC, isOn, arrowC));
    }

    // D�s�lectionner toutes les fl�ches (cacher les fl�ches)
    void DeselectArrows()
    {
        arrowA.SetActive(false);
        arrowB.SetActive(false);
        arrowC.SetActive(false);
    }

    // G�rer l'�tat du toggle et afficher la fl�che correspondante
    void OnToggleChanged(Toggle toggle, bool isOn, GameObject arrow)
    {
        if (isOn)
        {
            // Si le toggle est activ�, afficher la fl�che � c�t�
            arrow.SetActive(true);
        }
        else
        {
            // Si le toggle est d�sactiv�, cacher la fl�che
            arrow.SetActive(false);
        }
    }
}
