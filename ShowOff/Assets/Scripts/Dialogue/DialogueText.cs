using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class DialogueText
{
    public Image nucImage;
    public Image alexImage;
    public AudioClip sound;
    public TalkingCharacter currentCharacter;

    [TextArea(3, 10)]
    public string[] sentences;

    public enum TalkingCharacter
    {
        Nuc,
        Alex
    }

}
