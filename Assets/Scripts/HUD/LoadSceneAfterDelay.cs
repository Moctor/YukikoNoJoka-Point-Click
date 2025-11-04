using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneAfterDelay : MonoBehaviour
{
    [SerializeField]
    private float delayBeforeLoading = 3f;
    [SerializeField]
    private float delayBeforeSkip = 5f;
    [SerializeField]
    private string sceneNameToLoad;
    [SerializeField]
    private GameObject ButtonSkip;

    private float timeElapsed;

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > delayBeforeSkip)
        {
            ButtonSkip.SetActive(true);
        }

        if (timeElapsed > delayBeforeLoading)
        {
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }


}
