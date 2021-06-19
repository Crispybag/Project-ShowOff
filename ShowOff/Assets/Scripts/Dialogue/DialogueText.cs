using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// (Ezra) Contains the logic/settings for the basic dialogue
/// Seen/used by designers
/// </summary>


[System.Serializable]
public class DialogueText
{
    public DialogueManager.NucEmotes nucEmote;
    public DialogueManager.AlexEmotes alexEmote;
    //public DialogueManager.TalkingSounds sound;
    public AudioClip voiceLine;
    public TalkingCharacter currentTalkingCharacter;

    [TextArea(3, 10)]
    public string[] sentences;

    public enum TalkingCharacter
    {
        Nuc,
        Alex
    }



}
