using System.Collections;
using UnityEngine;

public class TextSpawn : MonoBehaviour
{
    [SerializeField] private GameObject p_text;
    [SerializeField] private float textSpawnTime = 3;
    [SerializeField] private string text;
    IEnumerator SpawnTime()
    {
        yield return new WaitForSeconds(textSpawnTime);
        p_text.GetComponent<TextFade>().text = text;
        Instantiate(p_text, this.transform);
    }

    void OnEnable()
    {
        StartCoroutine(SpawnTime());
    }
}
