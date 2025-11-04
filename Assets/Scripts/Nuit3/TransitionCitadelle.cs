using UnityEngine;


public class TransitionCitadelle : Object
{
    [SerializeField] private GameObject Citadelle;

    public void Transition()
    {
        gameObject.SetActive(false);
        Citadelle.SetActive(true);
        NextDialogue();
    }
}
