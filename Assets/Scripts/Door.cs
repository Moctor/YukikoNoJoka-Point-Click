using UnityEngine;

public class Door : Object,IInteractable
{
    public void OnInteract()
    {
        gameObject.SetActive(false);

        Debug.Log("Porte ouverte");

        NextDialogue();
    }

}
