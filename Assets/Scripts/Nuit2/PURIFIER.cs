using UnityEngine;

public class PURIFIER : Object, IInteractable
{
    public void OnInteract()
    {
        gameObject.SetActive(false);
        NextDialogue();
    }
}
