using UnityEngine;

public class Yukiko_Changer : MonoBehaviour
{
    private MaterialPropertyBlock mpb;
    private SpriteRenderer sr;

    //public yukikoClasse classe;
    //public yukikoYeux yeux;
    //public yukikoBouche bouche;

    [SerializeField] private float scale = 1;
    
    void Start()
    {
        mpb = new MaterialPropertyBlock();
        sr = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(scale, scale, 1);
    }
    /*private void Update()
    {
        Classe(classe);
        Yeux(yeux);
        Bouche(bouche);
    }*/
    public void Classe(yukikoClasse classe)
    {
        mpb.SetFloat("_Classe", (float)classe);
        sr.SetPropertyBlock(mpb);
    }

    public void Yeux(yukikoYeux yeux)
    {
        mpb.SetFloat("_Yeux_Emotions", (float)yeux);
        sr.SetPropertyBlock(mpb);
    }

    public void Bouche(yukikoBouche bouche)
    {
        mpb.SetFloat("_Bouche_Mouvement", (float)bouche);
        sr.SetPropertyBlock(mpb);
    }
}

public enum yukikoClasse
{
    artisan,
    pretress,
    samurai,
    marchand
}
public enum yukikoYeux
{
    neutre,
    heureuse,
    triste,
    colere,
    choque,
    triste_neutre,
    mourante_sang,
    furieuse
}

public enum yukikoBouche
{
    triste,
    normal,
    colere_choque,
    souriante,
    furieuse,
    mourante_sang,
    triste_neutre
}
