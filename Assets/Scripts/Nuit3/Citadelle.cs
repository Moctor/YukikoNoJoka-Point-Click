using UnityEngine;

public class Citadelle : Object,IInteractable
{
    [SerializeField] private GameObject ApparitionOniCitadelle;
    public void OnInteract()
    {
        ApparitionOniCitadelle.SetActive(false);
        gameObject.SetActive(false);
        NextDialogue();
    }
}
