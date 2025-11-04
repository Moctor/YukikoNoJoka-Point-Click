using UnityEngine;

public class LifeHUD : MonoBehaviour
{
    [SerializeField] private GameObject Life_1;
    [SerializeField] private GameObject Life_2;
    [SerializeField] private GameObject Life_3;

    // Update is called once per frame
    void Update()
    {
        LifeGestion();
    }

    private void LifeGestion()
    {
        if(PlayerPC.life ==1)
        {
            Life_1.SetActive(true);
            Life_2.SetActive(false);
            Life_3.SetActive(false);
        }
        if(PlayerPC.life ==2)
        {
            Life_2.SetActive(true);
            Life_3.SetActive(false);
            Life_1.SetActive(false);
        }
        if(PlayerPC.life ==3)
        {
            Life_3.SetActive(true);
            Life_1.SetActive(false);
            Life_2.SetActive(false);
        }
    }
}
