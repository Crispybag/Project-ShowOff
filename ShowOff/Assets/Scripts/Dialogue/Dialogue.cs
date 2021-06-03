using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [HideInInspector]public int ID;
    public Image nucImagePlace;
    public Image alexImagePlace;

    [SerializeField]
    private AudioSource talkingSound;

    public DialogueText[] dialogue;


    private Vector3 nucScale;
    private Vector3 alexScale;
    private int activeDialogue = 0;

    private DialogueManager _dialogueManager;

    private void Awake()
    {
        serviceLocator.interactableList.Add(ID, this.gameObject);
        _dialogueManager = FindObjectOfType<DialogueManager>();
        nucScale = nucImagePlace.transform.localScale;
        alexScale = alexImagePlace.transform.localScale;
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
                switch (dialogue[activeDialogue].currentCharacter)
                {
                    case (DialogueText.TalkingCharacter.Alex):
                        alexImagePlace.transform.localScale = alexScale;
                        nucImagePlace.transform.localScale = nucScale;
                        alexImagePlace.transform.localScale *= 1.25f;
                        break;
                    case (DialogueText.TalkingCharacter.Nuc):
                        alexImagePlace.transform.localScale = alexScale;
                        nucImagePlace.transform.localScale = nucScale;
                        nucImagePlace.transform.localScale *= 1.25f;
                        break;
                }
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
