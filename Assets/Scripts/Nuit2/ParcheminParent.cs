using UnityEngine;

public class ParcheminParent : Object
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
        if (ParcheminDecoupage.s_ParcheminDecoupe == 3)
        {
            gameObject.SetActive(false);
            MinigameDone = true;
            NextDialogue();
        }
    }
}
