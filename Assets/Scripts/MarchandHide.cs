using UnityEngine;

public class MarchandHide: MonoBehaviour
{
    [SerializeField] private GameObject Artisan;
    private void OnEnable()
    {
        Artisan.SetActive(false);
    }
}
