using UnityEngine;

public class OnizukaMap : Object,IInteractable
{
    [SerializeField] private GameObject HUD_Map;
    public void OnInteract()
    {
        NextDialogue();
        HUD_Map.SetActive(false);
    }

}
