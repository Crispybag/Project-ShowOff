using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationWriterDoor : InformationWriter
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void WriteAllInformation()
    {
        base.WriteAllInformation();
        //base.AddToInformation();
    }

}
