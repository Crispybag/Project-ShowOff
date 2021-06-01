using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public int ID;
    public bool isAwake = false;
    public Image nucImagePlace;
    public Image alexImagePlace;
    [SerializeField]
    private AudioSource talkingSound;

    public DialogueText[] dialogue;
    private int activeDialogue = 0;

    private DialogueManager _dialogueManager;

    private void Awake()
    {
        serviceLocator.interactableList.Add(ID, this.gameObject);
        _dialogueManager = FindObjectOfType<DialogueManager>();
        if (isAwake)
        {
            ProgressDialogue();
        }
    }

    public void ProgressDialogue()
    {
        if (activeDialogue <= dialogue.Length)
        {
            if (_dialogueManager.runningDialogue == false)
            {
                if (activeDialogue == dialogue.Length)
                {
                    _dialogueManager.EndDialogue();
                    return;
                    //Destroy(this.gameObject);
                }
                _dialogueManager.StartDialogue(dialogue[activeDialogue]);
                nucImagePlace.sprite = dialogue[activeDialogue].nucImage.sprite;
                alexImagePlace.sprite = dialogue[activeDialogue].alexImage.sprite;
                talkingSound.clip = dialogue[activeDialogue].sound;
                talkingSound.Play();
                activeDialogue++;
            }

            else if (_dialogueManager.runningDialogue == true)
            {
                //talkingSound.Play();
                _dialogueManager.DisplayNextSentence();
            }

        }

    }
}
