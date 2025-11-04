using System.Collections;
using UnityEngine;

public class ButtonVFX : MonoBehaviour
{
    [SerializeField] private GameObject p_vfx;
    [SerializeField] private float scale = 2;
    [SerializeField] private GameObject audioManager;
/*    [SerializeField] private bool m_buttonHover;
    [SerializeField] private bool m_buttonSelected;
*/
    private MaterialPropertyBlock mpb;
    private SpriteRenderer spRend;
    private bool isNull = false;
    private bool isSelected = false;
    private bool isCursed = false;
    void Start()
    {
        if (p_vfx != null)
        {
            isNull = false;
        }
        else
        {
            isNull = true;
        }
        isCursed = false;
        isSelected = false;
        transform.localScale = new Vector3(scale, scale, 1);
        mpb = new MaterialPropertyBlock();
        spRend = transform.GetComponent<SpriteRenderer>();
        NormalState();
    }

    public void NormalState()
    {
        mpb.SetFloat("_Hover", 0);
        mpb.SetFloat("_Selected", 0);
        mpb.SetFloat("_Cursed", 0);
        isCursed = false;
        isSelected = false;
        spRend.SetPropertyBlock(mpb);
        if (!isNull)
        {
            p_vfx.SetActive(false);
        }
    }

    private void HoverState()
    {
        mpb.SetFloat("_Hover", 1);
        mpb.SetFloat("_Selected", 0);
        mpb.SetFloat("_Cursed", 0);
        spRend.SetPropertyBlock(mpb);
        if (!isNull)
        {
            p_vfx.SetActive(true);
        }
    }
    public void SelectedState()
    {
        mpb.SetFloat("_Hover", 0);
        mpb.SetFloat("_Selected", 1);
        mpb.SetFloat("_Cursed", 0);
        StartCoroutine(FindFirstObjectByType<AudioManager>().PlaySound("Roue_Click"));
        isCursed = false;
        isSelected = true;
        spRend.SetPropertyBlock(mpb);
        if (!isNull)
        {
            p_vfx.SetActive(false);
        }
    }
    public void CursedState()
    {
        mpb.SetFloat("_Hover", 0);
        mpb.SetFloat("_Selected", 0);
        mpb.SetFloat("_Cursed", 1);
        isCursed = true;
        isSelected = false;
        spRend.SetPropertyBlock(mpb);
        if (!isNull)
        {
            p_vfx.SetActive(false);
        }
    }


    private void OnMouseOver()
    {
        if (!isCursed && !isSelected)
        {
            HoverState();
        }
    }

    private void OnMouseExit()
    {
        if (!isCursed && !isSelected)
        {
            NormalState();
            if (!isNull)
            {
                p_vfx.SetActive(false);
            };
        }
    }
}
