using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static DialogueTest;

public class DialogueTest : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float textSpeed = 0.05f;
    public System.Action OnLineFinished;

    [SerializeField] private AudioSource typingAudioSource;
    [SerializeField] private List<CharacterVoice> characterVoices = new List<CharacterVoice>();

    private Dictionary<string, List<AudioClip>> voiceDict = new Dictionary<string, List<AudioClip>>();
    private Queue<string> dialogueQueue = new Queue<string>();
    private string currentLine = "";
    private bool isTyping = false;
    private bool waitingForClick = false;

    private string currentSpeaker = "Default";

    [System.Serializable]
    public class CharacterVoice
    {
        public string characterName;
        public List<AudioClip> voiceClips = new List<AudioClip>();
    }

    private void Awake()
    {
        foreach (var entry in characterVoices)
        {
            if (!voiceDict.ContainsKey(entry.characterName))
            {
                voiceDict.Add(entry.characterName, entry.voiceClips);
            }
        }
    }

    private void Start()
    {
        textComponent.text = "";
    }

    public void SetCurrentSpeaker(string name)
    {
        currentSpeaker = name;
    }

    public void QueueLine(string line)
    {
        dialogueQueue.Enqueue(line);

        if (!isTyping)
        {
            DisplayNextLine();
        }
    }

    private void DisplayNextLine()
    {
        if (dialogueQueue.Count == 0)
        {
            textComponent.text = "";
            return;
        }

        currentLine = dialogueQueue.Dequeue();
        textComponent.text = "";
        StartCoroutine(TypeLine(currentLine));
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        waitingForClick = false;

        foreach (char c in line.ToCharArray())
        {
            textComponent.text += c;

            if (!char.IsWhiteSpace(c) && voiceDict.TryGetValue(currentSpeaker, out List<AudioClip> clips) && clips.Count > 0)
            {
                typingAudioSource.pitch = Random.Range(0.95f, 1.05f);
                typingAudioSource.PlayOneShot(clips[Random.Range(0, clips.Count)]);
            }

            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
        waitingForClick = true;
    }

    public void SkipOrNext()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            textComponent.text = currentLine;
            isTyping = false;
            waitingForClick = true;
        }
        else if (waitingForClick)
        {
            waitingForClick = false;
            OnLineFinished?.Invoke();
        }
        else
        {
            DisplayNextLine();
        }
    }

    public void ClearLine()
    {
        StopAllCoroutines();
        isTyping = false;
        waitingForClick = false;
        textComponent.text = "";
        currentLine = "";
        dialogueQueue.Clear();
    }

    public void ForceDisplayNextLine()
    {
        DisplayNextLine();
    }
}
