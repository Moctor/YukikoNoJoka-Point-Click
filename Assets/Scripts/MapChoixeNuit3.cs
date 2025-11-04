using UnityEngine;

public class MapChoixeNuit3 : MonoBehaviour
{
    [SerializeField] private GameObject MapP;
    [SerializeField] private GameObject MapR;
    private void Start()
    {
        if (PlayerPC.PurificatriceWay)
        {
            MapP.SetActive(true);
        }
        else
        {
            MapR.SetActive(true);
        }
    }
}
