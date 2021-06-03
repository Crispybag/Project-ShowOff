using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class DialogueText
{
    public DialogueManager.NucEmotes nucEmote;
    public DialogueManager.AlexEmotes alexEmote;
    public DialogueManager.TalkingSounds sound;
    public TalkingCharacter currentTalkingCharacter;

    [TextArea(3, 10)]
    public string[] sentences;

    public enum TalkingCharacter
    {
        Nuc,
        Alex
    }



}
