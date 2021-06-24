using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;
using UnityEngine.UI;


/// <summary>
/// (Ezra) Contains logic about the dialogue, what to show where
/// </summary>

public class Dialogue : MonoBehaviour
{
    [HideInInspector]public int ID;

    public DialogueText[] dialogue;


    private Vector3 nucScale;
    private Vector3 alexScale;
    private int activeDialogue = 0;

    private DialogueManager _dialogueManager;

    private void Awake()
    {
        serviceLocator.interactableList.Add(ID, this.gameObject);
        _dialogueManager = FindObjectOfType<DialogueManager>();
        nucScale = _dialogueManager.nucImagePlace.transform.localScale;
        alexScale = _dialogueManager.alexImagePlace.transform.localScale;
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

                setEmotes();
                setEmoteSizes();

                //playSound();
                _dialogueManager.talkingSound.clip = dialogue[activeDialogue].voiceLine;


                activeDialogue++;
            }

            else if (_dialogueManager.runningDialogue == true)
            {
                //talkingSound.Play();
                _dialogueManager.DisplayNextSentence();
            }

        }

    }

    //used previously for standard "hmm", "ah", "oh" sounds, now replaced by voicelines
    /*private void playSound()
    {
        switch (dialogue[activeDialogue].sound)
        {
            case (DialogueManager.TalkingSounds.AlexNeutral):
                _dialogueManager.talkingSound.clip = _dialogueManager.talkingSounds[0];
                break;
            case (DialogueManager.TalkingSounds.AlexHappy):
                _dialogueManager.talkingSound.clip = _dialogueManager.talkingSounds[1];
                break;
            case (DialogueManager.TalkingSounds.AlexThinking):
                _dialogueManager.talkingSound.clip = _dialogueManager.talkingSounds[2];
                break;
            case (DialogueManager.TalkingSounds.NucNeutral):
                _dialogueManager.talkingSound.clip = _dialogueManager.talkingSounds[3];
                break;
            case (DialogueManager.TalkingSounds.NucHappy):
                _dialogueManager.talkingSound.clip = _dialogueManager.talkingSounds[4];
                break;
            case (DialogueManager.TalkingSounds.NucThinking):
                _dialogueManager.talkingSound.clip = _dialogueManager.talkingSounds[5];
                break;
        }

        _dialogueManager.talkingSound.Play();
    }*/

    private void setEmoteSizes()
    {
        switch (dialogue[activeDialogue].currentTalkingCharacter)
        {
            case (DialogueText.TalkingCharacter.Alex):
                _dialogueManager.alexImagePlace.transform.localScale = alexScale;
                _dialogueManager.nucImagePlace.transform.localScale = nucScale;
                _dialogueManager.alexImagePlace.transform.localScale *= 1.25f;
                break;
            case (DialogueText.TalkingCharacter.Nuc):
                _dialogueManager.alexImagePlace.transform.localScale = alexScale;
                _dialogueManager.nucImagePlace.transform.localScale = nucScale;
                _dialogueManager.nucImagePlace.transform.localScale *= 1.25f;
                break;
        }
    }

    private void setEmotes()
    {
        switch (dialogue[activeDialogue].alexEmote)
        {
            case (DialogueManager.AlexEmotes.Neutral):
                _dialogueManager.alexImagePlace.sprite = _dialogueManager.alexEmotes[0].sprite;
                break;
            case (DialogueManager.AlexEmotes.Happy):
                _dialogueManager.alexImagePlace.sprite = _dialogueManager.alexEmotes[1].sprite;
                break;
            case (DialogueManager.AlexEmotes.Confused):
                _dialogueManager.alexImagePlace.sprite = _dialogueManager.alexEmotes[2].sprite;
                break;
            case (DialogueManager.AlexEmotes.Worried):
                _dialogueManager.alexImagePlace.sprite = _dialogueManager.alexEmotes[3].sprite;
                break;
            case (DialogueManager.AlexEmotes.Aye):
                _dialogueManager.alexImagePlace.sprite = _dialogueManager.alexEmotes[4].sprite;
                break;
        }

        switch (dialogue[activeDialogue].nucEmote)
        {
            case (DialogueManager.NucEmotes.Neutral):
                _dialogueManager.nucImagePlace.sprite = _dialogueManager.nucEmotes[0].sprite;
                break;
            case (DialogueManager.NucEmotes.Happy):
                _dialogueManager.nucImagePlace.sprite = _dialogueManager.nucEmotes[1].sprite;
                break;
            case (DialogueManager.NucEmotes.Confused):
                _dialogueManager.nucImagePlace.sprite = _dialogueManager.nucEmotes[2].sprite;
                break;
            case (DialogueManager.NucEmotes.Thinking):
                _dialogueManager.nucImagePlace.sprite = _dialogueManager.nucEmotes[3].sprite;
                break;
        }
    }


}
