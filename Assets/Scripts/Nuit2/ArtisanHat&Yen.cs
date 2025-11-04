using UnityEngine;

public class ArtisanHatYen : MonoBehaviour, IInteractable
{
    public DialogueRunner runner;

    public void OnInteract()
    {
        Classesroues.s_MarchantIsUnlocked = true;
        gameObject.SetActive(false);

        Debug.Log("Marchand débloqué. Passage au nœud suivant.");

        // Trouver le nœud suivant automatiquement
        var currentGUID = runner.GetCurrentNodeGUID();
        var next = runner.GetNextNode(currentGUID);

        if (!string.IsNullOrEmpty(next))
        {
            runner.ShowNodeByGUID(next);
        }
    }


}
