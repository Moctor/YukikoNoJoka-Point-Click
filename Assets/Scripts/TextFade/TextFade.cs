using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TextFade : MonoBehaviour
{
    [SerializeField] private GameObject p_text;
    [SerializeField] private TextMeshProUGUI textSceneChange;
    [SerializeField] private float speedFadeIn = 100;
    [SerializeField] private float speedFadeOut = 100;
    [SerializeField] private float timeSolidText = 3;

    public string text;

    private Color32 textColor;
    private float fadePourcentage;
    private bool fadeOut;
    private bool textSolid;

    void Start()
    {
        textSceneChange.text = text;
        textColor = textSceneChange.faceColor;
        textColor.a = 1;
        textSceneChange.faceColor = textColor;
        fadeOut = false;
        textSolid = false;
        fadePourcentage = 0;
    }
   
    void Update()
    {
        fadePourcentage += (!fadeOut) ? Time.deltaTime * speedFadeIn : Time.deltaTime * -speedFadeOut;
        fadePourcentage = Mathf.Clamp(fadePourcentage, -1f, 100f); 
        textColor.a = (byte)(fadePourcentage * 2.55f);

        if (fadePourcentage >= 100f && !textSolid)
        {
            textSolid = true;
            StartCoroutine(TextPause());
        }

        if (fadePourcentage <= 0)
        {
            Destroy(p_text);
        }

        textSceneChange.faceColor = textColor;
    }

    IEnumerator TextPause()
    {
        yield return new WaitForSeconds(timeSolidText);
        fadeOut = true;
        Debug.Log("Pausing text");
    }
}
