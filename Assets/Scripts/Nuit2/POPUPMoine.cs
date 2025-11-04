using System.Collections;
using UnityEngine;

public class POPUPMoine : Object
{
    [SerializeField] private float Delay;
    private void OnEnable()
    {
        Classesroues.s_MoineIsUnlocked = true;

        StartCoroutine(LifeTime());
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(Delay);
        NextDialogue();
        gameObject.SetActive(false);
    }
}
