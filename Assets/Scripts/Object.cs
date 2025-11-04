using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Object : MonoBehaviour
{
    public DialogueRunner runner;
    [SerializeField] private GameObject dialogueBox;
    public void Start()
    {
        dialogueBox.SetActive(false);
    }

    public void NextDialogue()
    {
        // Trouver le nœud suivant automatiquement
        var currentGUID = runner.GetCurrentNodeGUID();
        var next = runner.GetNextNode(currentGUID);
        dialogueBox.SetActive(true);


        if (!string.IsNullOrEmpty(next))
        {
            runner.ShowNodeByGUID(next);
        }
    }
}
