using System.Collections;
using UnityEngine;
using static Unity.Collections.Unicode;

public class BougieParent : Object
{
    private bool MinigameDone = false;
    // Update is called once per frame
    void Update()
    {
        if(!MinigameDone)
        {
            Minigame();
        }
    }

    private void Minigame()
    {
        if (BougieSpirituel.s_BougiesEteinte == 2)
        {
            gameObject.SetActive(false);
            Debug.Log("toutes éteintes");
            MinigameDone = true;
            NextDialogue();
        }
    }
}
