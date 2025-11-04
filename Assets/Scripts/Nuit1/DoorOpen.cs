using Unity.VisualScripting;
using UnityEngine;

public class DoorOpen : Object, IInteractable
{
    public GameObject Background1;
    public GameObject Background2;
    public void OnInteract()
    {
        Background1.SetActive(false);
        Background2.SetActive(true);
        gameObject.SetActive(false);

        Debug.Log("Porte ouverte");

        NextDialogue();
    }


}
