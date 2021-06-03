using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AirChannelManager))]
public class IWAirChannel : InformationWriter
{
    // Start is called before the first frame updatr
    private Vector3 airDirection;
    private float angle;
    public override void Start()
    {
        instantiateDirectionVectors();
        base.Start();
    }

    // Update is called once per frame
    public override void WriteAllInformation()
    {
        base.WriteAllInformation();
        AddToInformation(airDirection.x);
        AddToInformation(airDirection.y);
        AddToInformation(airDirection.z);
        AddToInformation(GetComponent<AirChannelManager>().ID);
        AddToInformation(GetComponent<AirChannelManager>().conditionsID);
    }

    private void instantiateDirectionVectors()
    {
        angle = Vector3.Angle(transform.TransformDirection(Vector3.forward), Vector3.forward);
        airDirection = Vector3.forward;
        
        compareAngle(-Vector3.forward);
        compareAngle( Vector3.left);
        compareAngle(-Vector3.left);
        compareAngle( Vector3.up);
        compareAngle(-Vector3.up);
    }

    private void compareAngle(Vector3 pVec)
    {
        float newAngle = Vector3.Angle(transform.TransformDirection(Vector3.forward), pVec);

        if (angle > newAngle)
        {
            angle = newAngle;
            airDirection = pVec;
        }
    }
}
