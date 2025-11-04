using UnityEngine;

public class Onizuka_Changer : MonoBehaviour
{
    private MaterialPropertyBlock mpb;
    private SpriteRenderer sr;

    //public onizukaYeux yeux;
    //public onizukaBouche bouche;

    [SerializeField] private float scale = 1;
    void Start()
    {
        mpb = new MaterialPropertyBlock();
        sr = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(scale, scale, 1);
    }

    /*private void Update()
    {
        Bouche(bouche);
        Yeux(yeux);
    }*/

    public void Bouche(onizukaBouche bouche) 
    {
        mpb.SetFloat("_Bouche", (float)bouche);
        sr.SetPropertyBlock(mpb);
    }

    public void Yeux(onizukaYeux yeux)
    {
        mpb.SetFloat("_Yeux", (float)yeux);
        sr.SetPropertyBlock(mpb);
    }
}

public enum onizukaYeux
{
    sigma,
    peur_inquiet,
    choque,    
    normal,
    ferme
}

public enum onizukaBouche
{
    normal,
    peur_stresse,
    silly_langue,
    choque,
    sourire,
    sigma
}