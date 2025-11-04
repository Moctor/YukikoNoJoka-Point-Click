using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class PlayerPC : MonoBehaviour
{
    public static int life = 3;
    public static int racisme;
    public static bool HasPayed = false;
    public static bool PurificatriceWay = false;
    [SerializeField] private GameObject gameover;


    private void Start()
    {
        // rendererRef = GetComponent<Renderer>();
        //  rendererRef.sharedMaterial = materials[0];
    }
    private void Update()
    {
        Samourai();
        Pretresse();
        Artisan();
        Marchand();
        PointAndClick();
        Death();
    }
    private void Samourai()
    {
        if (Classesroues.s_SamouraiIsSelectionned)
        {
        //    rendererRef.sharedMaterial = materials[1];
        }
    }
    private void Pretresse()
    {
        if (Classesroues.s_PretresseIsSelectionned)
        {
        //    rendererRef.sharedMaterial = materials[2];
        }
    }
    private void Artisan()
    {
        if (Classesroues.s_ArtisanIsSelectionned)
        {
        //    rendererRef.sharedMaterial = materials[3];
        }
    }
    private void Marchand()
    {
        if (Classesroues.s_MarchandIsSelectionned)
        {
        //    rendererRef.sharedMaterial = materials[4];
        }
    }

    private void PointAndClick()
    {
        if (Input.GetMouseButtonDown(0)) // clic gauche
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.OnInteract();
                }
            }
        }
    }

    private void Death()
    {
        if(life <= 0)
        {
            gameover.SetActive(true);
        }
    }
}
