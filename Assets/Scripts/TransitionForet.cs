using UnityEngine;
using System.Collections;
using static Unity.Collections.Unicode;


public class TransitionForet : MonoBehaviour
{
    public DialogueRunner runner;
    [SerializeField] private GameObject dialogueBox;
    Animator animator; 


    private void Start()
    {
        dialogueBox.SetActive(false);
    }
    private IEnumerator IETransition()
    {
        animator.SetTrigger("TransitionForet");
        yield return new WaitForSeconds(1.0f);
        NextDialogue();

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        animator = GetComponent<Animator>();

        StartCoroutine(IETransition());
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
