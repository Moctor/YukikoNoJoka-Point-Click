using UnityEngine;

public class MarchandActive : MonoBehaviour
{
    [SerializeField] private GameObject Artisan;
    private void OnEnable()
    {
        Artisan.SetActive(true);
    }
}
