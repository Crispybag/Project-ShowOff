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
    }
}
