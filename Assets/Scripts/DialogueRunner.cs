using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;
using UnityEngine.InputSystem.Interactions;

public class DialogueRunner : MonoBehaviour
{
    [Header("Dialogue Data")]
    [SerializeField] private TextContainer dialogueAsset;
    [SerializeField] private DialogueTest dialogueUI;

    [Header("UI Choix")]
    [SerializeField] private Transform choicesPanel;
    [SerializeField] private Button choiceButtonPrefab;

    private Dictionary<string, NodeData> nodes = new Dictionary<string, NodeData>();
    private Dictionary<string, List<LinksData>> links = new Dictionary<string, List<LinksData>>();
    private NodeData currentNode;
    private List<string> choicesMade = new List<string>();

    [SerializeField] private GameObject HUDExploration;
    [SerializeField] private GameObject HUDInteraction;
    [SerializeField] private GameObject HUDMap;
    [SerializeField] private GameObject roueDesClasses;


    [SerializeField] private GameObject dialogueBox; // ton UI principal
    [SerializeField] private RectTransform dialogueBoxTransform;
    [SerializeField] private Image dialogueBoxBackground;
    [SerializeField] private TMP_FontAsset Exemple_Kobajeon;
    [SerializeField] private TMP_FontAsset Exemple_Base;
    [SerializeField] private GameObject GoodEning;
    [SerializeField] private GameObject BadEning;
    [SerializeField] private GameObject SisBroEnding;
    [SerializeField] private GameObject GameOver;
    [SerializeField] private GameObject POMO;
    [SerializeField] private GameObject KOTARO;

    [SerializeField] private GameObject YukikoChanger;
    [SerializeField] private SpriteRenderer YukikoVisibility;
    //public yukikoYeux yeux;

    private bool Orrizuka = false;

    [SerializeField] private List<NamedObject> dynamicObjects = new List<NamedObject>();
    private Dictionary<string, GameObject> objectDict = new Dictionary<string, GameObject>();

    [SerializeField] private List<SpriteMapping> spriteMappings;
    private Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();

    [SerializeField] private List<NamedBackground> dynamicBackground;
    private Dictionary<string, GameObject> BackgroundDict = new Dictionary<string, GameObject>();

    [SerializeField] private List<NamedChoices> wayChoices;
    private Dictionary<string, GameObject> choiceDict = new Dictionary<string, GameObject>();

    [System.Serializable]
    public class SpriteMapping
    {
        public string key;       // ex: "Samourai"
        public Sprite sprite;    // image à afficher
    }

    [System.Serializable]
    public class ButtonMapping
    {
        public string key;       // Ex: "Provoquer_Samourai"
        public GameObject obj;
    }

    [System.Serializable]
    public class NamedBackground
    {
        public string background;
        public GameObject bgObj;
    }

    [System.Serializable]
    public class NamedChoices
    {
        public string choice;
        public GameObject wcObj;
    }

    [SerializeField]
    private List<ButtonMapping> buttonMappings = new List<ButtonMapping>();
    private Dictionary<string, GameObject> buttonDict = new Dictionary<string, GameObject>();

    private string currentButtonKeyBase = "";
    private void Start()
    {
        choicesMade.Clear(); // vide la liste à chaque nouveau dialogue
        LoadGraph();
        StartDialogue();
    }

    private void Awake()
    {
        foreach (var entry in dynamicObjects)
        {
            if (!objectDict.ContainsKey(entry.name))
                objectDict.Add(entry.name, entry.obj);
        }

        foreach (var entry in buttonMappings)
        {
            if (!buttonDict.ContainsKey(entry.key))
                buttonDict.Add(entry.key, entry.obj);
        }
        foreach (var entry in dynamicBackground)
        {
            if (!BackgroundDict.ContainsKey(entry.background))
            {
                BackgroundDict.Add(entry.background, entry.bgObj);
            }
        }
        foreach (var entry in wayChoices)
        {
            if (!choiceDict.ContainsKey(entry.choice))
            {
                choiceDict.Add(entry.choice, entry.wcObj);
            }
        }
    }

    public void RefreshActiveButton()
    {
        if (string.IsNullOrEmpty(currentButtonKeyBase)) return;

        foreach (var entry in buttonMappings)
        {
            if (entry.obj != null)
                entry.obj.SetActive(false);
        }
        string classKey = "";
        if (Classesroues.s_SamouraiIsSelectionned) classKey = "Samourai";
        else if (Classesroues.s_PretresseIsSelectionned) classKey = "Pretresse";
        else if (Classesroues.s_ArtisanIsSelectionned) classKey = "Artisan";
        else if (Classesroues.s_MarchandIsSelectionned) classKey = "Marchand";

        string finalKey = currentButtonKeyBase + "_" + classKey;

        if (buttonDict.TryGetValue(finalKey, out GameObject obj) && obj != null)
            obj.SetActive(true);
    }
    private void LoadGraph()
    {
        foreach (var node in dialogueAsset.nodesData)
            nodes[node.nodeGUID] = node;

        foreach (var link in dialogueAsset.linksData)
        {
            if (!links.ContainsKey(link.originGUID))
                links[link.originGUID] = new List<LinksData>();

            links[link.originGUID].Add(link);
        }

        currentNode = dialogueAsset.nodesData.Find(n => n.dialogueText == "ENTRYPOINT");
        if (currentNode != null && links.ContainsKey(currentNode.nodeGUID))
        {
            string nextGUID = links[currentNode.nodeGUID][0].targetGUID;
            if (nodes.ContainsKey(nextGUID))
                currentNode = nodes[nextGUID];
        }
    }

    private void StartDialogue()
    {
        ShowNode(currentNode);
    }
    public void RefreshClasseChoices()
    {
        if (this.currentNode != null && this.currentNode.dialogueText == "CHOIX SELON CLASSE")
        {
            DisplayChoicesIfNeeded(this.currentNode);
        }
    }
    private void ShowNode(NodeData node)
    {
        if (node == null)
        {
            Debug.LogError("ShowNode a recu un noeud null.");
            return;
        }
        if (node.dialogueText.StartsWith("EMOTION "))
        {

            string emotion = node.dialogueText.Replace("EMOTION ", "").Trim().ToLower();
            Debug.Log("il print ça : " + emotion);
            if (Classesroues.s_SamouraiIsSelectionned)
            {
                YukikoChanger.GetComponent<Yukiko_Changer>().Classe(yukikoClasse.samurai);
            }
            
            else if (Classesroues.s_MarchandIsSelectionned)
            {
                YukikoChanger.GetComponent<Yukiko_Changer>().Classe(yukikoClasse.marchand);
            }
            else if (Classesroues.s_ArtisanIsSelectionned)
            {
                YukikoChanger.GetComponent<Yukiko_Changer>().Classe(yukikoClasse.artisan);
            }
            else
            {
                YukikoChanger.GetComponent<Yukiko_Changer>().Classe(yukikoClasse.pretress);
            }
            switch (emotion)
            {
                case "heureuse":
                    YukikoChanger.GetComponent<Yukiko_Changer>().Yeux(yukikoYeux.heureuse);
                    YukikoChanger.GetComponent<Yukiko_Changer>().Bouche(yukikoBouche.souriante);
                    break;
                case "stressee":
                    YukikoChanger.GetComponent<Yukiko_Changer>().Yeux(yukikoYeux.choque);
                    YukikoChanger.GetComponent<Yukiko_Changer>().Bouche(yukikoBouche.colere_choque);
                    break;
                case "colere":
                    YukikoChanger.GetComponent<Yukiko_Changer>().Yeux(yukikoYeux.colere);
                    YukikoChanger.GetComponent<Yukiko_Changer>().Bouche(yukikoBouche.colere_choque);
                    break;
                case "furieuse":
                    YukikoChanger.GetComponent<Yukiko_Changer>().Yeux(yukikoYeux.furieuse);
                    YukikoChanger.GetComponent<Yukiko_Changer>().Bouche(yukikoBouche.furieuse);
                    break;
                case "triste":
                    YukikoChanger.GetComponent<Yukiko_Changer>().Yeux(yukikoYeux.triste);
                    YukikoChanger.GetComponent<Yukiko_Changer>().Bouche(yukikoBouche.triste);
                    break;
                case "neutre":
                    YukikoChanger.GetComponent<Yukiko_Changer>().Yeux(yukikoYeux.neutre);
                    YukikoChanger.GetComponent<Yukiko_Changer>().Bouche(yukikoBouche.normal);
                    break;
                default:
                    Debug.LogWarning("Emotion non reconnue : " + emotion);
                    break;
            }

            // sauter automatiquement au prochain noeud s’il n’y en a qu’un
            if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
            {
                string nextGUID = links[node.nodeGUID][0].targetGUID;
                if (nodes.ContainsKey(nextGUID))
                {
                    currentNode = nodes[nextGUID];
                    Classesroues.s_ClasseSelectionActive = false;
                    ShowNode(currentNode);

                }
            }
            return;

    }

        if (node.dialogueText.StartsWith("BG "))
        {
            string background = node.dialogueText.Replace("BG ", "").Trim();

            if (BackgroundDict.TryGetValue(background, out GameObject bgObj) && bgObj != null)
            {

                bgObj.SetActive(true);
                if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
                {
                    string nextGUID = links[node.nodeGUID][0].targetGUID;
                    if (nodes.ContainsKey(nextGUID))
                    {
                        currentNode = nodes[nextGUID];
                        ShowNode(currentNode);
                    }
                }
                Debug.Log("Background : " + background);
                return;
            }
            else
            {
                Debug.LogWarning("BACKGROUND Introuvable : " + background);
                return;
            }
        }
        if (node.dialogueText.StartsWith("WC "))
        {

            foreach (var entry in wayChoices)
            {
                if (entry.wcObj != null)
                    entry.wcObj.SetActive(false);
            }
            string choice = node.dialogueText.Replace("WC ", "").Trim();

            if (choiceDict.TryGetValue(choice, out GameObject wcObj) && wcObj != null)
            {

                wcObj.SetActive(true);

                if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
                {
                    string nextGUID = links[node.nodeGUID][0].targetGUID;
                    if (nodes.ContainsKey(nextGUID))
                    {
                        currentNode = nodes[nextGUID];
                        ShowNode(currentNode);
                    }
                }
                Debug.Log("Choice : " + choice);
                return;
            }
            else
            {
                Debug.LogWarning("Choice Introuvable : " + choice);
                return;
            }

        }
        // SHOW NODE POUR OBJET
        if (node.dialogueText.StartsWith("Y"))
        {
            Debug.Log("C'est Yukiko qui parle");
            dialogueUI.textComponent.font = Exemple_Kobajeon;
            dialogueUI.SetCurrentSpeaker("Yukiko");

        }
        else
        {
            Debug.Log("C'est pas Yukiko qui parle");
            dialogueUI.textComponent.font = Exemple_Base;
            dialogueUI.SetCurrentSpeaker("Autres");
        }
        if (node.dialogueText.StartsWith("SHOW "))
        {
            string key = node.dialogueText.Replace("SHOW ", "").Trim();

            if (objectDict.TryGetValue(key, out GameObject obj) && obj != null)
            {
                obj.SetActive(true);
                Debug.Log("SHOW -> " + key);
                return;
            }
            else
            {
                Debug.LogWarning("SHOW -> Objet introuvable : " + key);
                return;
            }

        }
        if (node.dialogueText.StartsWith("LOAD SCENE "))
        {
            string sceneName = node.dialogueText.Replace("LOAD SCENE ", "").Trim();

            Debug.Log("Chargement de la scène : " + sceneName);
            SceneManager.LoadScene(sceneName);
            return;
        }
        //Saut automatique : CHOIX SELON CLASSE
        if (node.dialogueText == "CHOIX SELON CLASSE")
        {
            Classesroues.s_ClasseSelectionActive = true;

            dialogueUI.QueueLine("Choisissez une classe.");
            currentNode = node;

            dialogueUI.OnLineFinished = () =>
            {
                if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
                {
                    string nextGUID = links[node.nodeGUID][0].targetGUID;
                    if (nodes.ContainsKey(nextGUID))
                    {
                        currentNode = nodes[nextGUID];
                        ShowNode(currentNode);
                    }
                }
                else
                {
                    DisplayChoicesIfNeeded(node);
                }
            };

            return;
        }

        if (node.dialogueText.StartsWith("BUTTON "))
        {
            Classesroues.s_ClasseSelectionActive = true;

            foreach (var entry in buttonMappings)
                if (entry.obj != null)
                    entry.obj.SetActive(false);

            string keyBase = node.dialogueText.Replace("BUTTON ", "").Trim();
            currentButtonKeyBase = keyBase;

            RefreshActiveButton();
            return;
        }

        // Saut automatique : SET HUD
        if (node.dialogueText.StartsWith("SET HUD "))
        {
            string targetHUD = node.dialogueText.Replace("SET HUD ", "").Trim();
            SetHUD(targetHUD);
            


            // sauter automatiquement au prochain noeud s’il n’y en a qu’un
            if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
            {
                string nextGUID = links[node.nodeGUID][0].targetGUID;
                if (nodes.ContainsKey(nextGUID))
                {
                    currentNode = nodes[nextGUID];
                    Classesroues.s_ClasseSelectionActive = false;
                    ShowNode(currentNode);

                }
            }

            return;
        }
        


        // Verification si les conditions sont remplies pour déclencher le dialogue secret
        if (node.dialogueText == "SECRET DIALOGUE")
        {
            bool hasChoiceA = choicesMade.Contains("9ec2f461-c115-47b5-a11c-5053520d5884");
            bool hasChoiceB = choicesMade.Contains("c981e4e6-1794-4398-85fb-e9d55ab46c04");
            Orrizuka = true;

            if (hasChoiceA && hasChoiceB) // si on a fait les deux choix nous redirige avec le dialogue secret 
            {
                Debug.Log("Conditions remplies : redirection vers le dialogue secret.");
                currentNode = nodes["92fdaab3-7dcd-4376-91cb-aeca742cade1"]; // nous met au node du dialogue secret
                Classesroues.s_ClasseSelectionActive = false;
                ShowNode(currentNode);
            }
            else // nous permet de continuer le déroulement des dialogue
            {
                Debug.Log("Conditions pas remplies : continuer normalement.");
                if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
                {
                    currentNode = nodes[links[node.nodeGUID][0].targetGUID];
                    Classesroues.s_ClasseSelectionActive = false;
                    ShowNode(currentNode);
                }
            }
            return;
        }
        // verif des conditions pour le dialogue de fukuro
        if (node.dialogueText == "FUKURO DIALOGUE")
        {
            bool hasChoiceA = choicesMade.Contains("43ed5f77-630a-4ed9-aaa0-22b0564d2022");
            bool hasChoiceB = choicesMade.Contains("9e5beb88-110a-4df8-86a5-71c9f420337f");

            if (hasChoiceA && hasChoiceB && Classesroues.s_PretresseIsSelectionned) // si on a fait les deux choix nous redirige avec le dialogue secret 
            {
                Debug.Log("Conditions remplies : redirection vers le dialogue secret.");
                currentNode = nodes["24d5c776-2433-464b-9cf4-8c88cfd81e8c"]; // nous met au node du dialogue voulu
                Classesroues.s_ClasseSelectionActive = false;
                ShowNode(currentNode);
            }
            else // nous permet de continuer le déroulement des dialogue
            {
                Debug.Log("Conditions pas remplies : continuer normalement.");
                if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
                {
                    currentNode = nodes[links[node.nodeGUID][0].targetGUID];
                    Classesroues.s_ClasseSelectionActive = false;
                    ShowNode(currentNode);
                }
            }
            return;
        }
        //verif pour la voie de la purificatrice
        if (node.dialogueText == "PURIFICATRICE WAY SKIP")
        {

            bool hasChoice = choicesMade.Contains("52330aff-9746-49e3-b524-80fec5e2abe8");
            PlayerPC.PurificatriceWay = true;

            if (hasChoice)
            {
                Debug.Log("Conditions remplies : redirection vers le dialogue secret.");
                currentNode = nodes["559c0a9d-96db-4fac-a61e-88b739db6778"]; // nous met au node du dialogue voulu
                Classesroues.s_ClasseSelectionActive = false;
                ShowNode(currentNode);
            }
            else // nous permet de continuer le déroulement des dialogue
            {
                Debug.Log("Conditions pas remplies : continuer normalement.");
                if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
                {
                    currentNode = nodes[links[node.nodeGUID][0].targetGUID];
                    Classesroues.s_ClasseSelectionActive = false;
                    ShowNode(currentNode);
                }
            }
            return;
        }

        if (node.dialogueText == "PURIFICATRICE WAY")
        {
            if (PlayerPC.PurificatriceWay)
            {
                
                currentNode = nodes["78fbba53-bc22-4f26-aa34-42fbcc5b5650"]; // nous met au node du dialogue voulu
                Classesroues.s_ClasseSelectionActive = false;
                ShowNode(currentNode);
            }
            else // nous permet de continuer le déroulement des dialogue
            {
                Debug.Log("Conditions pas remplies : continuer normalement.");
                if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
                {
                    currentNode = nodes[links[node.nodeGUID][0].targetGUID];
                    Classesroues.s_ClasseSelectionActive = false;
                    ShowNode(currentNode);
                }
            }
            return;
        }

        if (node.dialogueText == "ORIZZUKA")
        {
            if (Orrizuka)
            {
                currentNode = nodes["b58ffef5-590e-4c08-a051-4dc2b55aa353"]; // nous met au node du dialogue voulu
                ShowNode(currentNode);
            }
            else // nous permet de continuer le déroulement des dialogue
            {
                Debug.Log("Conditions pas remplies : continuer normalement.");
                if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
                {
                    currentNode = nodes[links[node.nodeGUID][0].targetGUID];
                    ShowNode(currentNode);
                }
            }
            return;
        }
        // verif si notre racisme est a 0
        if (node.dialogueText == "RACISME0")
        {
            if (PlayerPC.racisme == 0)
            {
                currentNode = nodes["e53a5f00-6f5d-4ad7-8407-1ab5d2a0d07c"]; // nous met au node du dialogue voulu
                ShowNode(currentNode);
            }
            else // nous permet de continuer le déroulement des dialogue
            {
                Debug.Log("Conditions pas remplies : continuer normalement.");
                if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
                {
                    currentNode = nodes[links[node.nodeGUID][0].targetGUID];
                    ShowNode(currentNode);
                }
            }
            return;
        }
        if (node.dialogueText == "RACISMEFINAL0PRIER")
        {
            if (PlayerPC.racisme == 0)
            {
                currentNode = nodes["ac8886a1-ecbb-4899-9179-69d9204b504c"]; // nous met au node du dialogue voulu
                ShowNode(currentNode);
            }
            else // nous permet de continuer le déroulement des dialogue
            {
                Debug.Log("Conditions pas remplies : continuer normalement.");
                if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
                {
                    currentNode = nodes[links[node.nodeGUID][0].targetGUID];
                    ShowNode(currentNode);
                }
            }
            return;
        }
        //verif si on choisit de tuer alors qu'on a choisit la voie de la purification
        if (node.dialogueText == "CHOIX FINAL TUER")
        {
            if (PlayerPC.racisme == 0)
            {
                currentNode = nodes["d6f615da-35d0-4e74-b0fa-b28494d85a77"]; // nous met au node du dialogue voulu
                ShowNode(currentNode);
            }
            else // nous permet de continuer le déroulement des dialogue
            {
                Debug.Log("Conditions pas remplies : continuer normalement.");
                if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
                {
                    currentNode = nodes[links[node.nodeGUID][0].targetGUID];
                    ShowNode(currentNode);
                }
            }
            return;
        }
        if (node.dialogueText == "EVILFINAL")
        {
            if (PlayerPC.racisme == 3)
            {
                currentNode = nodes["52837788-34c3-4a9a-aefd-5364d13ccf74"]; // nous met au node du dialogue voulu
                ShowNode(currentNode);
            }
            else // nous permet de continuer le déroulement des dialogue
            {
                Debug.Log("Conditions pas remplies : continuer normalement.");
                if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
                {
                    currentNode = nodes[links[node.nodeGUID][0].targetGUID];
                    ShowNode(currentNode);
                }
            }
            return;
        }
        // verif si notre valeur de racisme est de 1
        if (node.dialogueText == "RACISME1")
        {
            if (PlayerPC.racisme == 1)
            {
                currentNode = nodes["0912dd4c-c812-4671-b6e4-ef07999ba849"]; // nous met au node du dialogue voulu
                ShowNode(currentNode);
            }
            else // nous permet de continuer le déroulement des dialogue
            {
                Debug.Log("Conditions pas remplies : continuer normalement.");
                if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
                {
                    currentNode = nodes[links[node.nodeGUID][0].targetGUID];
                    ShowNode(currentNode);
                }
            }
            return;
        }
        // verif si notre valeur de racisme est de 2
        if (node.dialogueText == "RACISME2")
        {
            if (PlayerPC.racisme == 2)
            {
                currentNode = nodes["ca75b8d0-b9c7-4d01-aeae-b74031b07b2c"]; // nous met au node du dialogue voulu
                ShowNode(currentNode);
            }
            else // nous permet de continuer le déroulement des dialogue
            {
                Debug.Log("Conditions pas remplies : continuer normalement.");
                if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
                {
                    currentNode = nodes[links[node.nodeGUID][0].targetGUID];
                    ShowNode(currentNode);
                }
            }
            return;
        }

        if(node.dialogueText == "ENDING GOOD")
        {
            GoodEning.SetActive(true);
        }
        if(node.dialogueText == "GAMEOVER")
        {
            GameOver.SetActive(true);
        }
        if(node.dialogueText == "ENDING BAD")
        {
            BadEning.SetActive(true);
        }
        if(node.dialogueText == "ENDING EVIL")
        {
            SisBroEnding.SetActive(true);
        }

        if (node.dialogueText == "PURIFICATRICE MAP")
        {
            if (PlayerPC.PurificatriceWay)
            {
                currentNode = nodes["e2b598e1-a5f9-4707-b6b7-0bb64a8dfbf3"]; // nous met au node du dialogue voulu
                Classesroues.s_ClasseSelectionActive = false;
                ShowNode(currentNode);
            }
            else // nous permet de continuer le déroulement des dialogue
            {
                Debug.Log("Conditions pas remplies : continuer normalement.");
                if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
                {
                    currentNode = nodes[links[node.nodeGUID][0].targetGUID];
                    Classesroues.s_ClasseSelectionActive = false;
                    ShowNode(currentNode);
                }
            }
            return;
        }
        //maudit la classe pretresse
        if (node.dialogueText == "CURSE PRETRESSE"!)
        {
            Classesroues.s_PretresseIsCursed = true;

            // enchaine automatiquement si un seul lien sortant
            if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
            {
                string nextGUID = links[node.nodeGUID][0].targetGUID;
                if (nodes.ContainsKey(nextGUID))
                {
                    currentNode = nodes[nextGUID];
                    Classesroues.s_ClasseSelectionActive = false;
                    ShowNode(currentNode);

                    Classesroues.s_ShouldSkipCheck = true;
                    var cr = roueDesClasses.GetComponent<Classesroues>();
                    if (cr != null)
                    {
                        Classesroues.ResetClasse(cr); // passe la référence pour le reset complet
                    }
                }
            }

            return;
        }

        if(node.dialogueText == "POMO")
        {
            POMO.SetActive(false);
            KOTARO.SetActive(true);
        }
        //maudit la classe samourai
        if (node.dialogueText == "CURSE SAMOURAI")
        {
            Classesroues.s_PretresseIsCursed = true;
            Classesroues.s_SamouraiIsCursed = true;
            Classesroues.s_ShouldSkipCheck = true;
            var cr = roueDesClasses.GetComponent<Classesroues>();
            if (cr != null)
            {
                Classesroues.ResetClasse(cr); // passe la référence pour le reset complet
            }

            // enchaine automatiquement si un seul lien sortant
            if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
            {
                string nextGUID = links[node.nodeGUID][0].targetGUID;
                if (nodes.ContainsKey(nextGUID))
                {
                    currentNode = nodes[nextGUID];
                    Classesroues.s_ClasseSelectionActive = false;
                    ShowNode(currentNode);


                }
            }
            return;
        }
        //maudit la classe artisan&marchand et reactive samourai&pretresse
        if (node.dialogueText == "CURSE FINAL")
        {
            Classesroues.s_MarchandIsCursed = true;
            Classesroues.s_ArtisanIsCursed = true;
            Classesroues.s_SamouraiIsCursed = false;
            Classesroues.s_PretresseIsCursed = false;
            Classesroues.s_PretresseIsUnlocked = true;

            // enchaine automatiquement si un seul lien sortant
            if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
            {
                string nextGUID = links[node.nodeGUID][0].targetGUID;
                if (nodes.ContainsKey(nextGUID))
                {
                    currentNode = nodes[nextGUID];
                    Classesroues.s_ClasseSelectionActive = false;
                    ShowNode(currentNode);
                }
            }
            return;
        }
        // verif quand le joueur perds une vie
        if (node.dialogueText == "LOSE LIFE")
        {
            PlayerPC.life--;
            Debug.Log("Perte de vie déclenchée via Event Node. Vies restantes : " + PlayerPC.life);

            // Enchaîner automatiquement si un seul lien sortant
            if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
            {
                string nextGUID = links[node.nodeGUID][0].targetGUID;
                if (nodes.ContainsKey(nextGUID))
                {
                    currentNode = nodes[nextGUID];
                    Classesroues.s_ClasseSelectionActive = false;
                    ShowNode(currentNode);
                }
            }

            return;
        }
        // verif quand le joueur fait un choix qui augmente sa valeur de racisme
        if (node.dialogueText == "RACISME")
        {
            PlayerPC.racisme++;
            Debug.Log("Perte de vie déclenchée via Event Node. Vies restantes : " + PlayerPC.racisme);

            // enchaine automatiquement si un seul lien sortant
            if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
            {
                string nextGUID = links[node.nodeGUID][0].targetGUID;
                if (nodes.ContainsKey(nextGUID))
                {
                    currentNode = nodes[nextGUID];
                    Classesroues.s_ClasseSelectionActive = false;
                    ShowNode(currentNode);
                }
            }

            return;
        }

        if (node.dialogueText == "HAS PAYED")
        {
            PlayerPC.HasPayed = true;

            // enchaine automatiquement si un seul lien sortant
            if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
            {
                string nextGUID = links[node.nodeGUID][0].targetGUID;
                if (nodes.ContainsKey(nextGUID))
                {
                    currentNode = nodes[nextGUID];
                    Classesroues.s_ClasseSelectionActive = false;
                    ShowNode(currentNode);
                }
            }

            return;
        }

        if (node.dialogueText == "PAID DIALOGUE")
        {
            if (PlayerPC.HasPayed) //check si le on a utiliser l'argent
            {
                Debug.Log("Conditions remplies : redirection vers la selection de classe");
                currentNode = nodes["21b08c55-5102-4b05-a5bf-6dc4deb58eed"]; // nous met au node du dialogue voulu
                Classesroues.s_ClasseSelectionActive = false;
                ShowNode(currentNode);
            }
            else // nous permet de continuer le déroulement des dialogue
            {
                Debug.Log("Conditions pas remplies : continuer normalement.");
                if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
                {
                    currentNode = nodes[links[node.nodeGUID][0].targetGUID];
                    Classesroues.s_ClasseSelectionActive = false;
                    ShowNode(currentNode);
                }
            }
            return;
        }

        // Affichage standard
        ClearChoices();
        dialogueUI.QueueLine(node.dialogueText);

        dialogueUI.OnLineFinished = () =>
        {
            if (links.ContainsKey(node.nodeGUID) && links[node.nodeGUID].Count == 1)
            {
                string nextGUID = links[node.nodeGUID][0].targetGUID;
                if (nodes.ContainsKey(nextGUID))
                {
                    currentNode = nodes[nextGUID];
                    ShowNode(currentNode);
                }
            }
            else
            {
                DisplayChoicesIfNeeded(node);
            }
        };

    }

    private void DisplayChoicesIfNeeded(NodeData node)
    {

        // Cas spécial : redirection automatique selon la classe du joueur
        if (node.dialogueText == "CHOIX SELON CLASSE")
        {
            string portToFollow = "";

            if (Classesroues.s_SamouraiIsSelectionned)
                portToFollow = "Samourai";
            else if (Classesroues.s_PretresseIsSelectionned)
                portToFollow = "Pretresse";
            else if (Classesroues.s_MarchandIsSelectionned)
                portToFollow = "Marchand";
            else if (Classesroues.s_ArtisanIsSelectionned)
                portToFollow = "Artisan";

            if (string.IsNullOrEmpty(portToFollow))
            {
                // Aucun choix fait -> bloquer ici
                Debug.Log("Aucune classe selectionnee. En attente...");
                dialogueUI.QueueLine("Veuillez choisir une classe.");
                currentNode = node;
                return;
            }
            Debug.Log("Port recherché : " + portToFollow);
            foreach (var link in links[node.nodeGUID])
            {
                Debug.Log("Lien trouvé - Port : " + link.portName + " vers " + link.targetGUID);
            }
            var target = links[node.nodeGUID].Find(link => link.portName == portToFollow);

            if (target != null && nodes.ContainsKey(target.targetGUID))
            {
                if (!choicesMade.Contains(target.targetGUID))
                {
                    choicesMade.Add(target.targetGUID);
                    Debug.Log("Choix sélectionné, GUID : " + (target.targetGUID));   // permet de voir le GUID du dialogue ou on va
                }
                currentNode = nodes[target.targetGUID];
                Classesroues.s_ClasseSelectionActive = false;
                ShowNode(currentNode); // saute l'affichage du node intermédiaire
            }
            else
            {
                Debug.LogError("Lien manquant pour la classe : " + portToFollow);
            }

            return;
        }



        // Cas normal : affichage des choix s'il y en a plusieurs
        if (!links.ContainsKey(node.nodeGUID))
            return;

        List<LinksData> choices = links[node.nodeGUID];

        if (choices.Count == 1)
        {
            StartCoroutine(AdvanceAfterDelay(0.01f, choices[0].targetGUID));
        }
        else
        {
            foreach (var link in choices)
            {
                Button newBtn = Instantiate(choiceButtonPrefab, choicesPanel);
                newBtn.gameObject.SetActive(true);

                string label = !string.IsNullOrEmpty(link.textOption)
                    ? link.textOption
                    : (nodes.ContainsKey(link.targetGUID) ? nodes[link.targetGUID].dialogueText : "[Choix]");

                var textComp = newBtn.GetComponentInChildren<TextMeshProUGUI>();
                if (textComp != null) textComp.text = label;

                Image img = newBtn.GetComponentInChildren<Image>();
                if (img != null && spriteDict.TryGetValue(link.portName, out Sprite sp))
                {
                    img.sprite = sp;
                    img.enabled = true;
                }

                newBtn.onClick.AddListener(() =>
                {
                    dialogueUI.ClearLine();
                    ClearChoices();
                    OnChoiceSelected(link.targetGUID);
                });
            }
        }
    }

    private IEnumerator AdvanceAfterDelay(float delay, string nextGUID)
    {
        yield return new WaitForSeconds(delay);

        var link = links[currentNode.nodeGUID].Find(l => l.targetGUID == nextGUID);

        if (nodes.ContainsKey(nextGUID))
        {
            currentNode = nodes[nextGUID];
            Classesroues.s_ClasseSelectionActive = false;
            ShowNode(currentNode);
        }
        else
        {
            Debug.LogError("GUID introuvable après délai : " + nextGUID);
        }
    }


    private void OnChoiceSelected(string targetGUID)
    {
        if (nodes.ContainsKey(targetGUID))
        {
            var link = links[currentNode.nodeGUID].Find(l => l.targetGUID == targetGUID);

            currentNode = nodes[targetGUID];
            Classesroues.s_ClasseSelectionActive = false;
            ShowNode(currentNode);
        }
        else
        {
            Debug.LogError("GUID cible non trouvé : " + targetGUID);
        }
    }

    private void ClearChoices()
    {
        foreach (Transform child in choicesPanel)
        {
            Destroy(child.gameObject);
        }
    }

    private void SetHUD(string name)
    {
        Debug.Log("Changement de HUD vers : " + name);

        switch (name)
        {
            case "EXPLORATION":
                YukikoChanger.SetActive(false);
                HUDExploration.SetActive(true);
                HUDMap.SetActive(false);
                HUDInteraction.SetActive(false);
                roueDesClasses.SetActive(false);
                YukikoVisibility.color = new Color(1f, 1f, 1f, 0f);
                dialogueBoxBackground.color = new Color(0f, 0f, 0f, 0.87f);
                dialogueBoxTransform.anchorMin = new Vector2(0.5f, 1f);
                dialogueBoxTransform.anchorMax = new Vector2(0.5f, 1f);
                dialogueBoxTransform.pivot = new Vector2(0.5f, 0.5f);
                dialogueBoxTransform.sizeDelta = new Vector2(1000f, 300f);
                dialogueBoxTransform.anchoredPosition = new Vector2(17f, -915f);

                break;
            case "MAP":
                HUDExploration.SetActive(false);
                HUDMap.SetActive(true);
                HUDInteraction.SetActive(false);
                roueDesClasses.SetActive(false);
                YukikoVisibility.color = new Color(1f, 1f, 1f, 0f);
                dialogueBoxTransform.anchorMin = new Vector2(0.5f, 1f);
                dialogueBoxTransform.anchorMax = new Vector2(0.5f, 1f);
                dialogueBoxTransform.pivot = new Vector2(0.5f, 0.5f);
                dialogueBoxTransform.sizeDelta = new Vector2(1000f, 300f);
                dialogueBoxTransform.anchoredPosition = new Vector2(17f, -915f);
                break;
            case "INTERACTION":
                Classesroues.s_ShouldSkipCheck = true;
                var cr = roueDesClasses.GetComponent<Classesroues>();
                if (cr != null)
                {
                    Classesroues.ResetClasse(cr); // passe la référence pour le reset complet
                }
                HUDExploration.SetActive(false);
                HUDMap.SetActive(false);
                HUDInteraction.SetActive(true);
                roueDesClasses.SetActive(true);
                YukikoVisibility.color = new Color(1f, 1f, 1f, 1f);
                dialogueBoxBackground.color = new Color(0f, 0f, 0f, 0f);
                dialogueBoxTransform.anchorMin = new Vector2(0.5f, 1f);
                dialogueBoxTransform.anchorMax = new Vector2(0.5f, 1f);
                dialogueBoxTransform.pivot = new Vector2(0.5f, 0.5f);
                dialogueBoxTransform.sizeDelta = new Vector2(1000f, 300f);
                dialogueBoxTransform.anchoredPosition = new Vector2(444f, -915f);


                break;
            default:
                Debug.LogWarning("HUD inconnu : " + name);
                break;
        }
    }

    public void StartDialogueFromNode(string guid)
    {
        if (nodes.ContainsKey(guid))
        {
            currentNode = nodes[guid];
            Classesroues.s_ClasseSelectionActive = false;
            ShowNode(currentNode);
        }
        else
        {
            Debug.LogError("GUID de noeud inconnu : " + guid);
        }
    }

    public string GetCurrentNodeGUID()
    {
        return currentNode != null ? currentNode.nodeGUID : "";
    }

    public string GetNextNode(string guid)
    {
        if (links.ContainsKey(guid) && links[guid].Count == 1)
            return links[guid][0].targetGUID;

        return "";
    }

    public void ShowNodeByGUID(string guid)
    {
        if (nodes.ContainsKey(guid))
        {
            currentNode = nodes[guid];
            Classesroues.s_ClasseSelectionActive = false;
            ShowNode(currentNode);
        }   
    }

    [System.Serializable]
    public class NamedObject
    {
        public string name;
        public GameObject obj;
    }

}