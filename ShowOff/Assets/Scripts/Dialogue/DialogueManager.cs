using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public Text NPCnameText;
    public Text dialogueText;
    public GameObject panel;

    private Queue<string> sentences = new Queue<string>();
    public bool runningDialogue = false;


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

}
