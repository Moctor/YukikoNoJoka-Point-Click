using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.Unicode;

public class Emotions : MonoBehaviour
{
    public DialogueRunner runner;

    private void OnEnable()
    {

        gameObject.SetActive(true);


        Debug.Log("Emotion changée");

        // NextDialogue();

    }

    public void NextDialogue()
    {
        // Trouver le nœud suivant automatiquement
        var currentGUID = runner.GetCurrentNodeGUID();
        var next = runner.GetNextNode(currentGUID);

        if (!string.IsNullOrEmpty(next))
        {
            runner.ShowNodeByGUID(next);
        }
    }

}
