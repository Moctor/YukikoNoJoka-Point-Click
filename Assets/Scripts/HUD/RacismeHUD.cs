using UnityEngine;

public class RacismeHUD : MonoBehaviour
{
    [SerializeField] private GameObject Racisme_1;
    [SerializeField] private GameObject Racisme_2;
    [SerializeField] private GameObject Racisme_3;

    private void Update()
    {
        
    }

    private void GestionRacisme()
    {
        if(PlayerPC.racisme == 1)
        {
            Racisme_1.SetActive(true);
            Racisme_2.SetActive(false);
            Racisme_3.SetActive(false);
        }
        if(PlayerPC.racisme == 2)
        {
            Racisme_1.SetActive(false);
            Racisme_2.SetActive(true);
            Racisme_3.SetActive(false);
        }
        if (PlayerPC.racisme == 3)
        {
            Racisme_1.SetActive(false);
            Racisme_2.SetActive(false);
            Racisme_3.SetActive(true);
        }
    }
}
