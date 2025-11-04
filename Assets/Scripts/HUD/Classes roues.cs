using UnityEngine;
using UnityEngine.UI;

public class Classesroues : MonoBehaviour
{
    [SerializeField] private GameObject Samurai;
    [SerializeField] private GameObject Pretresse;
    [SerializeField] private GameObject Artisan;
    [SerializeField] private GameObject Marchand;
    [SerializeField] private Image Samourai_Gris;
    [SerializeField] private Image Samourai_Vert;
    [SerializeField] private Image Pretresse_Gris;
    [SerializeField] private Image Pretresse_Vert;
    [SerializeField] private Image Artisan_Gris;
    [SerializeField] private Image Artisan_Vert;
    [SerializeField] private Image Marchand_Gris;
    [SerializeField] private Image Marchand_Vert;
    [SerializeField] private RectTransform Circle_Hud;
    [SerializeField] private GameObject Classes;
    [SerializeField] private GameObject MoineImage;
    [SerializeField] private DialogueRunner dialogueRunner;

    [SerializeField] private float Rotation_Speed;
    [SerializeField] private float Angle = 0.0f;
    private float Target_Angle = 0.0f;
    private bool isRotating = false;

    public static bool s_SamouraiIsUnlocked = false;
    public static bool s_PretresseIsUnlocked = false;
    public static bool s_ArtisanIsUnlocked = false;
    public static bool s_MarchantIsUnlocked = false;
    public static bool s_MoineIsUnlocked = false;

    public static bool s_SamouraiIsSelectionned = false;
    public static bool s_PretresseIsSelectionned = false;
    public static bool s_ArtisanIsSelectionned = false;
    public static bool s_MarchandIsSelectionned = false;

    public static bool s_SamouraiIsCursed = false;
    public static bool s_PretresseIsCursed = false;
    public static bool s_ArtisanIsCursed = false;
    public static bool s_MarchandIsCursed = false;

    public static bool s_ShouldSkipCheck = false;
    public static bool s_ClasseSelectionActive = false;

    private ButtonVFX samuraiOption;
    private ButtonVFX pretresseOption;
    private ButtonVFX artisanOption;
    private ButtonVFX marchandOption;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        samuraiOption = Samurai.GetComponent<ButtonVFX>();
        pretresseOption = Pretresse.GetComponent<ButtonVFX>();
        artisanOption = Artisan.GetComponent<ButtonVFX>();
        marchandOption = Marchand.GetComponent<ButtonVFX>();
        Samourai_Vert.enabled = false;
        Pretresse_Vert.enabled = false;
        Artisan_Vert.enabled = false;
        Marchand_Vert.enabled = false;
        Samourai_Gris.enabled = true;
        Pretresse_Gris.enabled = true;
        Artisan_Gris.enabled = true;
        Marchand_Gris.enabled = true;
        Angle = 0.0f;
        Target_Angle = 0.0f;
        s_PretresseIsUnlocked = true;
        isRotating = false;
        /*s_SamouraiIsUnlocked = true;
        s_MarchantIsUnlocked = true;
        s_ArtisanIsUnlocked = true;*/
        CheckSamourai();
        CheckPretresse();
        CheckMarchand();
        CheckArtisan();
    }

    // Update is called once per frame
    void Update()
    {
        if (s_MoineIsUnlocked)
        {
            Moine();
        }
        if (isRotating)
        {
            Angle = Mathf.MoveTowardsAngle(Angle, Target_Angle, Rotation_Speed * Time.deltaTime); //trouve le chemin le plus rapide entre angle et target angle
            Circle_Hud.localRotation = Quaternion.Euler(0f, 0f, Angle);

            RotationImage();

            if (Mathf.Approximately(Angle, Target_Angle)) // verifie si les deux valeurs sont asser proches ou identiques pour terminer la rotation
            {
                isRotating = false;
            }
        }
    }
    public void SamouraiOnClicked()
    {
        if (!s_ClasseSelectionActive) return;
        {
            if (!isRotating)
            {
                if (s_SamouraiIsUnlocked)
                {
                    if (!s_SamouraiIsCursed)
                    {
                        samuraiOption.SelectedState();
                        pretresseOption.NormalState();
                        artisanOption.NormalState();
                        marchandOption.NormalState();
                        /*
                        Samourai_Gris.enabled = false;
                        Pretresse_Gris.enabled = true;
                        Artisan_Gris.enabled = true;
                        Marchand_Gris.enabled = true;
                        Samourai_Vert.enabled = true;
                        Pretresse_Vert.enabled = false;
                        Artisan_Vert.enabled = false;
                        Marchand_Vert.enabled = false;*/
                        RotationPosition(1);
                        s_SamouraiIsSelectionned = true;
                        s_PretresseIsSelectionned = false;
                        s_ArtisanIsSelectionned = false;
                        s_MarchandIsSelectionned = false;
                    }
                    else
                    {
                        ResetClasse();
                        Debug.Log("cette Classe est maudite");
                    }

                    if (dialogueRunner != null)
                        dialogueRunner.RefreshClasseChoices();
                    //dialogueRunner.RefreshActiveButton();
                }
            }
        }
    }
    public void PretresseOnClicked()
    {
        if (!s_ClasseSelectionActive) return;
        {
            if (!isRotating)
            {
                if (s_PretresseIsUnlocked)
                {
                    if (!s_PretresseIsCursed)
                    {
                        samuraiOption.NormalState();
                        pretresseOption.SelectedState();
                        artisanOption.NormalState();
                        marchandOption.NormalState();
                        /*
                        Samourai_Gris.enabled = true;
                        Pretresse_Gris.enabled = false;
                        Artisan_Gris.enabled = true;
                        Marchand_Gris.enabled = true;
                        Samourai_Vert.enabled = false;
                        Pretresse_Vert.enabled = true;
                        Artisan_Vert.enabled = false;
                        Marchand_Vert.enabled = false;*/
                        RotationPosition(2);
                        s_SamouraiIsSelectionned = false;
                        s_PretresseIsSelectionned = true;
                        s_ArtisanIsSelectionned = false;
                        s_MarchandIsSelectionned = false;
                    }
                    else
                    {
                        ResetClasse();
                        Debug.Log("cette Classe est maudite");
                    }

                    if (dialogueRunner != null)
                        dialogueRunner.RefreshClasseChoices();
                    // dialogueRunner.RefreshActiveButton();
                }
            }
        }
    }
    public void MarchandOnClicked()
    {
        if (!s_ClasseSelectionActive) return;
        {
            if (!isRotating)
            {
                if (s_MarchantIsUnlocked)
                {
                    if (!s_MarchandIsCursed)
                    {
                        samuraiOption.NormalState();
                        pretresseOption.NormalState();
                        artisanOption.NormalState();
                        marchandOption.SelectedState();/*
                        Samourai_Gris.enabled = true;
                        Pretresse_Gris.enabled = true;
                        Artisan_Gris.enabled = true;
                        Marchand_Gris.enabled = false;
                        Samourai_Vert.enabled = false;
                        Pretresse_Vert.enabled = false;
                        Artisan_Vert.enabled = false;
                        Marchand_Vert.enabled = true;*/
                        RotationPosition(3);
                        s_SamouraiIsSelectionned = false;
                        s_PretresseIsSelectionned = false;
                        s_ArtisanIsSelectionned = false;
                        s_MarchandIsSelectionned = true;
                    }
                    else
                    {
                        ResetClasse();
                        Debug.Log("cette Classe est maudite");
                    }

                    if (dialogueRunner != null)
                        dialogueRunner.RefreshClasseChoices();
                }
            }
        }
    }
    public void ArtisanOnClicked()
    {
        if (!s_ClasseSelectionActive) return;
        {
            if (!isRotating)
            {
                if (s_ArtisanIsUnlocked)
                {
                    if (!s_ArtisanIsCursed)
                    {
                        samuraiOption.NormalState();
                        pretresseOption.NormalState();
                        artisanOption.SelectedState();
                        marchandOption.NormalState();
                        Samourai_Gris.enabled = true;/*
                        Pretresse_Gris.enabled = true;
                        Artisan_Gris.enabled = false;
                        Marchand_Gris.enabled = true;
                        Samourai_Vert.enabled = false;
                        Pretresse_Vert.enabled = false;
                        Artisan_Vert.enabled = true;
                        Marchand_Vert.enabled = false;*/
                        RotationPosition(4);
                        s_SamouraiIsSelectionned = false;
                        s_PretresseIsSelectionned = false;
                        s_ArtisanIsSelectionned = true;
                        s_MarchandIsSelectionned = false;
                    }
                    else
                    {
                        Debug.Log("cette Classe est maudite");
                        ResetClasse();
                    }

                    if (dialogueRunner != null)
                        dialogueRunner.RefreshClasseChoices();
                }
            }
        }
    }
    private void RotationPosition(int possibility)
    {
        float[] angles = { 0f, 90f, 180f, 270f };  // pretresse, samourai, marchand, artisan ce sont leurs premieres rotations qu'ils doivent faire
        Target_Angle = angles[possibility - 1]; // donne la rotation qui est cibl� 
        isRotating = true;
    }

    private void RotationImage()
    {
        // faire tourner les images en sens inverse
        float inverseAngle = -Angle;

        Samurai.transform.localRotation = Quaternion.Euler(0f, 0f, inverseAngle);
        Artisan.transform.localRotation = Quaternion.Euler(0f, 0f, inverseAngle);
        Pretresse.transform.localRotation = Quaternion.Euler(0f, 0f, inverseAngle);
        Marchand.transform.localRotation = Quaternion.Euler(0f, 0f, inverseAngle);

        Samourai_Gris.rectTransform.localRotation = Quaternion.Euler(0f, 0f, inverseAngle);
        Samourai_Vert.rectTransform.localRotation = Quaternion.Euler(0f, 0f, inverseAngle);
        Pretresse_Gris.rectTransform.localRotation = Quaternion.Euler(0f, 0f, inverseAngle);
        Pretresse_Vert.rectTransform.localRotation = Quaternion.Euler(0f, 0f, inverseAngle);
        Artisan_Gris.rectTransform.localRotation = Quaternion.Euler(0f, 0f, inverseAngle);
        Artisan_Vert.rectTransform.localRotation = Quaternion.Euler(0f, 0f, inverseAngle);
        Marchand_Gris.rectTransform.localRotation = Quaternion.Euler(0f, 0f, inverseAngle);
        Marchand_Vert.rectTransform.localRotation = Quaternion.Euler(0f, 0f, inverseAngle);
    }

    private void CheckSamourai()//lors qu'on change de scene permet de verifier si on avait une classe sElectionner avant
    {
        if (s_SamouraiIsSelectionned)
        {
            Samourai_Vert.enabled = true;
            Samourai_Gris.enabled = false;
            samuraiOption.SelectedState();
            RotationPosition(1);
            RotationImage();
            s_PretresseIsSelectionned = false;
            s_MarchandIsSelectionned = false;
            s_ArtisanIsSelectionned = false;
        }
    }
    private void CheckPretresse()//lors qu'on change de scene permet de verifier si on avait une classe s�lectionner avant
    {
        if (s_PretresseIsSelectionned)
        {
            Pretresse_Vert.enabled = true;
            Pretresse_Gris.enabled = false;
            pretresseOption.SelectedState();
            RotationPosition(2);
            RotationImage();
            s_MarchandIsSelectionned = false;
            s_SamouraiIsSelectionned = false;
            s_ArtisanIsSelectionned = false;
        }
    }
    private void CheckMarchand()//lors qu'on change de scene permet de verifier si on avait une classe s�lectionner avant
    {
        if (s_MarchandIsSelectionned)
        {
            Marchand_Vert.enabled = true;
            Marchand_Gris.enabled = false;
            marchandOption.SelectedState();
            RotationPosition(3);
            RotationImage();
            s_PretresseIsSelectionned = false;
            s_SamouraiIsSelectionned = false;
            s_ArtisanIsSelectionned = false;
        }
    }
    private void CheckArtisan()//lors qu'on change de scene permet de verifier si on avait une classe s�lectionner avant
    {
        if (s_ArtisanIsSelectionned)
        {
            Artisan_Vert.enabled = true;
            Artisan_Gris.enabled = false;
            artisanOption.SelectedState();
            RotationPosition(4);
            RotationImage();
            s_PretresseIsSelectionned = false;
            s_SamouraiIsSelectionned = false;
            s_MarchandIsSelectionned = false;
        }
    }

    public static void ResetClasse(Classesroues instance = null)
    {
        s_SamouraiIsSelectionned = false;
        s_PretresseIsSelectionned = false;
        s_ArtisanIsSelectionned = false;
        s_MarchandIsSelectionned = false;

        if (instance != null)
        {
            instance.ResetClasseVisuals();
        }
    }
    public void ResetClasseVisuals()
    {
        Samourai_Vert.enabled = false;
        Pretresse_Vert.enabled = false;
        Artisan_Vert.enabled = false;
        Marchand_Vert.enabled = false;

        Samourai_Gris.enabled = true;
        Pretresse_Gris.enabled = true;
        Artisan_Gris.enabled = true;
        Marchand_Gris.enabled = true;

        samuraiOption.NormalState();
        pretresseOption.NormalState();
        artisanOption.NormalState();
        marchandOption.NormalState();
    }

    private void Moine() //reset les classes choisie affiche l'image du moine et d�sactive la roue des classes
    {
        if (s_MoineIsUnlocked)
        {
            ResetClasse();
            MoineImage.SetActive(true);
            Classes.SetActive(false);
        }
    }
}
