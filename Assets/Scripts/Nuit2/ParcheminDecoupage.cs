using UnityEngine;

public class ParcheminDecoupage : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject Haut_Parchemin;
    [SerializeField] private Collider2D boxCollider2D;
    public static int s_ParcheminDecoupe;
    public void OnInteract()
    {

        Haut_Parchemin.SetActive(false);
        boxCollider2D.enabled = false;
        Debug.Log("Parchemin Decoupe");
        s_ParcheminDecoupe++;
        Debug.Log(s_ParcheminDecoupe);

    }
}
