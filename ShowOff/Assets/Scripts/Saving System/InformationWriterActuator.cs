using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationWriterActuator : InformationWriter
{
    public override void Start()
    {
        base.Start();
    }
    public override void WriteAllInformation()
    {
        base.WriteAllInformation();
        AddToInformation(this.gameObject.GetComponent<PuzzleFactory>().ID);
        AddToInformation(this.gameObject.GetComponent<PuzzleFactory>().interactableIDs);
    }
}
