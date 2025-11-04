using UnityEngine;

public class OnizukaDoor : Object, IInteractable
{
    [SerializeField] private GameObject Background;
    [SerializeField] private GameObject Background2;
    public void OnInteract()
    {
        Background.SetActive(false);
        Background2.SetActive(true);
        NextDialogue();
    }
}
