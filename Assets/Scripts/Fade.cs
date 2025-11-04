using System.Collections;
using UnityEngine;
using System;
using static Unity.Collections.Unicode;

public class Fade : MonoBehaviour
{
    Animator animator;
    public float WaitTime = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Fading(Action onFadeOutComplete)
    {
        StartCoroutine(Fader( onFadeOutComplete));
        
    }

    private IEnumerator Fader(Action onFadeOutComplete)
    {
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(WaitTime);
        onFadeOutComplete?.Invoke(); // execute l'action de changement ce qui me permet de lancer un code apres la premiere partie de macoroutine
        animator.SetTrigger("FadeIn");
    }
}
