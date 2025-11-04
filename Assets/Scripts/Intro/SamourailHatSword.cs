using Unity.VisualScripting;
using UnityEngine;

public class SamourailHatSword : Object, IInteractable
{


    public void OnInteract()
    {
        Classesroues.s_SamouraiIsUnlocked = true;
        gameObject.SetActive(false);

        Debug.Log("Samourai débloqué. Passage au nœud suivant.");

        NextDialogue();

    }
}
