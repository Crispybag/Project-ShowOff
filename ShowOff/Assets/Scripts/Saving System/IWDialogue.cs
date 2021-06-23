using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (Ezra) Writes necessary information about the dialogue mechanic
/// </summary>

public class IWDialogue : InformationWriter
{
    public override void StartSave()
    {
        base.StartSave();
    }

    public override void WriteAllInformation()
    {
        base.WriteAllInformation();
        AddToInformation(GetComponent<Dialogue>().ID);
        AddToInformation(createDialogue());
    }

    private List<Vector3> createDialogue()
    {
        List<Vector3> dialogue = new List<Vector3>();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            dialogue.Add(this.transform.GetChild(i).transform.position);
        }
        return dialogue;
    }
}
