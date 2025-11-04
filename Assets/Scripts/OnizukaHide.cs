using UnityEngine;

public class OnizukaHide : MonoBehaviour
{
    [SerializeField] private GameObject Onizuka;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Onizuka.SetActive(false);   
    }

}
