using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IWAirChannel : InformationWriter
{

    [SerializeField] private Vector3 airDirection;
    // Start is called before the first frame updatr
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void WriteAllInformation()
    {
        base.WriteAllInformation();
        AddToInformation(airDirection.x);
        AddToInformation(airDirection.y);
        AddToInformation(airDirection.z);
    }
}
