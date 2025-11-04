using Unity.VisualScripting;
using UnityEngine;

public class Marteau : Object, IInteractable
{
    
    public void OnInteract()
    {
        gameObject.SetActive(false);

        Debug.Log("Lien du marteau coupé");

        NextDialogue();
    }

}
