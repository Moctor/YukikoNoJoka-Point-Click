using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueClickZone : MonoBehaviour, IPointerClickHandler
{
    public DialogueTest dialogue;

    public void OnPointerClick(PointerEventData eventData)
    {
        dialogue.SkipOrNext();
    }
}
