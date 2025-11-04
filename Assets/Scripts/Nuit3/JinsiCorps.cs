using UnityEngine;

public class JinsiCorps : Object, IInteractable
{
    public void OnInteract()
    {
        gameObject.SetActive(false);
        NextDialogue();
    }
}
