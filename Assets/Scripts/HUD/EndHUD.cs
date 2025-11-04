using UnityEngine;
using UnityEngine.SceneManagement;

public class EndHUD : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;

    private void OnEnable()
    {
        dialogueBox.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0); //retour au main menu
    }

    public void Quit()
    {
        Application.Quit();
    }
}
