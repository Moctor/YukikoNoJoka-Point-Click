using UnityEngine;

public class Marchand_Change : MonoBehaviour
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

    public void MarchandYeuxNormal()
    {
        mpb.SetFloat("_Yeux", 0f);
        sr.SetPropertyBlock(mpb);
    }

    public void MarchandYeuxCreepy()
    {
        mpb.SetFloat("_Yeux", 1f);
        sr.SetPropertyBlock(mpb);
    }
    public void MarchandBoucheNormal()
    {
        mpb.SetFloat("_Bouche", 1f);
        sr.SetPropertyBlock(mpb);
    }

    public void MarchandBoucheSourire()
    {
        mpb.SetFloat("_Bouche", 0f);
        sr.SetPropertyBlock(mpb);
    }
}
