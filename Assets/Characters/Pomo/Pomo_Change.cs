using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class Pomo_Change : MonoBehaviour
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

    public void BoucheSourire()
    {
        mpb.SetFloat("_Bouche_Mouvement", 0f);
        sr.SetPropertyBlock(mpb);
    }

    public void BoucheChoque()
    {
        mpb.SetFloat("_Bouche_Mouvement", 1f);
        sr.SetPropertyBlock(mpb);
    }
}
