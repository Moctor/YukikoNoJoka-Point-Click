using System.Collections;
using UnityEngine;

public class POPUP : Object
{
    [SerializeField] private float Delay;
    private void OnEnable()
    {
        StartCoroutine(LifeTime());
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(Delay);
        NextDialogue();
        gameObject.SetActive(false);
    }
}
