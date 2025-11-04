using UnityEngine;

public class YukikoActive : MonoBehaviour
{
    [SerializeField] private GameObject yukiko;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        yukiko.SetActive(true);
    }
}
