using UnityEngine;

public class Talisman : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject Whole;
    [SerializeField] GameObject Cut;
    public static int s_TalismanCutted;
    private bool TalismanCanbeCut;

    private void Start()
    {
        TalismanCanbeCut = true;
    }
    public void OnInteract()
    {
        if (Classesroues.s_SamouraiIsSelectionned & TalismanCanbeCut)
        {
            Debug.Log("J'interrargis");
            Whole.SetActive(false);
            Cut.SetActive(true);
            s_TalismanCutted++;
            TalismanCanbeCut = false;
            Debug.Log(s_TalismanCutted);
        }
    }
}
