using UnityEngine;

public class Purifier : Object, IInteractable
{
    public void OnInteract()
    {
        gameObject.SetActive(false);
        NextDialogue();
    }
}
