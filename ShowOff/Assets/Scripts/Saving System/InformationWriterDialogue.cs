using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationWriterDialogue : InformationWriter
{
    public override void Start()
    {
        base.Start();
    }

    public override void WriteAllInformation()
    {
        base.WriteAllInformation();
        AddToInformation(this.GetComponent<Dialogue>().ID);
    }
}
