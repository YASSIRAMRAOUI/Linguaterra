using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableLetter : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private int originalIndex;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalIndex = transform.GetSiblingIndex();
        canvasGroup.blocksRaycasts = false;

        GetComponent<Button>().interactable = false;
        Debug.Log("Drag commencé !");
    }

    public void OnDrag(PointerEventData eventData)
    {
        // NE RIEN FAIRE : on laisse l'objet dans le layout
        // donc on ne modifie pas sa position
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        GetComponent<Button>().interactable = true;

        // On peut choisir de NE PAS remettre à l’index d’origine, 
        // pour garder la nouvelle position dans le layout
        // Sinon, pour annuler le drag, décommente :
        // transform.SetSiblingIndex(originalIndex);
    }
}
