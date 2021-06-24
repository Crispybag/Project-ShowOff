using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// (Ezra) Contains most information and logic for the dialogue. 
/// </summary>

public class DialogueManager : MonoBehaviour
{

    public Text NPCnameText;
    public Text dialogueText;
    public GameObject panel;

    public Image nucImagePlace;
    public Image alexImagePlace;

    [SerializeField]
    public AudioSource talkingSound;

    private Queue<string> sentences = new Queue<string>();
    public bool runningDialogue = false;

    [Tooltip("Neutral, Happy, Confused, Thinking")]public Image[] nucEmotes;
    [Tooltip("Neutral, Happy, Confused, Worried, Aye")]public Image[] alexEmotes;
    [Tooltip("AlexNeutral, AlexHappy, AlexThinking, NucNeutral, NucHappy, NucThinking")] public AudioClip[] talkingSounds;

    void Awake()
    {
        sentences = new Queue<string>();
        panel.SetActive(false);
        runningDialogue = false;
    }


    public void StartDialogue(DialogueText dialogue)
    {
        panel.SetActive(true);
        runningDialogue = true;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        if (sentences.Count == 0)
        {
            runningDialogue = false;
            return;
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.025f);
        }
    }


    public void EndDialogue()
    {
        panel.SetActive(false);
        runningDialogue = false;
    }

    public enum NucEmotes
    {
        Neutral = 0,
        Happy = 1,
        Confused = 2,
        Thinking = 3
    }

    public enum AlexEmotes
    {
        Neutral = 0,
        Happy = 1,
        Confused = 2,
        Worried = 3,
        Aye = 4
    }

    public enum TalkingSounds
    {
        AlexNeutral,
        AlexHappy,
        AlexThinking,
        NucNeutral,
        NucHappy,
        NucThinking
    }

}
