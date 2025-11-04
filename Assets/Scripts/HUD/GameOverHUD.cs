using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHUD : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;

    private void OnEnable()
    {
        dialogueBox.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //restart la scene ou on est
    }

    public void Quit()
    {
        Application.Quit();
    }
}
