using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationWriterSlope : InformationWriter
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void WriteAllInformation()
    {
        base.WriteAllInformation();
        AddToInformation(transform.rotation.eulerAngles.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
