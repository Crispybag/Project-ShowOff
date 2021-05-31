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
    public string NPCname;

    [TextArea(3, 10)]
    public string[] sentences;
}
