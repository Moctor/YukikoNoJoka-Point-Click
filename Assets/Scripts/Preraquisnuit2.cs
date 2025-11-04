using UnityEngine;

public class Preraquisnuit2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Classesroues.s_MarchantIsUnlocked = true;
        Classesroues.s_SamouraiIsUnlocked = true;
        if (PlayerPC.PurificatriceWay)
        {
            Classesroues.s_ArtisanIsUnlocked = true;

        }
    }

}
