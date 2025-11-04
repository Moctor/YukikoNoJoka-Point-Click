using UnityEngine;

public class ForetHiboux_BG : MonoBehaviour
{
    public DialogueRunner runner;
    private void OnEnable()
    {

        gameObject.SetActive(true);
        Debug.Log("Bg changée");
        //NextDialogue();

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
