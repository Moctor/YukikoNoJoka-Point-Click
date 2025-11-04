using UnityEngine;

public class HideMarchand : MonoBehaviour
{

    [SerializeField] private GameObject Marchand;
    private void OnEnable()
    {
        Marchand.SetActive(false);
    }
}
