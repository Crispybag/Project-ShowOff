using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationWriterDialogue : InformationWriter
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
