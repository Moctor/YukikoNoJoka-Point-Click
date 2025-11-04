using UnityEngine;

public class Moine_Change : MonoBehaviour
{
    private MaterialPropertyBlock mpb;
    private SpriteRenderer sr;

    [SerializeField] private float scale = 1;
    void Start()
    {
        mpb = new MaterialPropertyBlock();
        sr = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(scale, scale, 1);
    }

    public void MoineNormal()
    {
        mpb.SetFloat("_Change", 0f);
        sr.SetPropertyBlock(mpb);
    }

    public void MoineAttaque()
    {
        mpb.SetFloat("_Change", 1f);
        sr.SetPropertyBlock(mpb);
    }
}
