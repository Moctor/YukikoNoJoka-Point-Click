using UnityEngine;
using UnityEngine.SceneManagement;
public class SkipButton : MonoBehaviour, IInteractable
{
    [SerializeField]
    private string sceneNameToLoad;

    public void OnInteract()
    {
        Debug.Log("Allo");
        SceneManager.LoadScene(sceneNameToLoad);

    }
    

}
