using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped != null)
        {
            dropped.transform.SetParent(transform);

            // Insérer au bon endroit dans le layout
            dropped.transform.SetSiblingIndex(GetIndexFromPointer(eventData));
        }
    }

    private int GetIndexFromPointer(PointerEventData eventData)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform child = transform.GetChild(i) as RectTransform;
            if (eventData.position.x < child.position.x)
            {
                return i;
            }
        }
        return transform.childCount;
    }
}
