using System.Collections;
using UnityEngine;

public class BougieSpirituel : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject Feu_Spirituel;
    [SerializeField] private Collider2D boxCollider2D;
    public static int s_BougiesEteinte;
    [SerializeField] private float totalTime = 2;

    public void OnInteract()
    {
        if (Classesroues.s_PretresseIsSelectionned)
        {
            Feu_Spirituel.SetActive(false);
            boxCollider2D.enabled = false;
            Debug.Log("Le feu est éteint");
            s_BougiesEteinte++;
            Debug.Log(s_BougiesEteinte);

        }
    }

    private void Update()
    {
        if (totalTime > 0)
        {
            //Subtract elapsed time every frame
            totalTime -= Time.deltaTime;
        }
        else
        {
            Feu_Spirituel.SetActive(true);
            boxCollider2D.enabled = true;
            Debug.Log("Le feu se rallume");
            s_BougiesEteinte--;
            if (s_BougiesEteinte <= 0)
            {
                s_BougiesEteinte = 0;
            }
            totalTime = 2;
        }

    }

}