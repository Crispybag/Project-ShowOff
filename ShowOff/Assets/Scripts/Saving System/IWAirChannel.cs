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
        base.Start();
    }

    // Update is called once per frame
    public override void WriteAllInformation()
    {
        instantiateDirectionVectors();
        base.WriteAllInformation();
        AddToInformation(airDirection.x);
        AddToInformation(airDirection.y);
        AddToInformation(airDirection.z);
        AddToInformation(GetComponent<AirChannelManager>().ID);
        AddToInformation(createList(GetComponent<AirChannelManager>().conditions));
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

    private List<int> createList(List<GameObject> gameobjects)
    {
        List<int> IDs = new List<int>();
        foreach (GameObject obj in gameobjects)
        {
            if (obj.GetComponentInChildren<PuzzleFactory>() != null)
            {
                Debug.Log("Found a puzzle factory item in a list!");
                IDs.Add(obj.GetComponentInChildren<PuzzleFactory>().ID);
            }
            else if (obj.GetComponentInChildren<DoorManager>() != null)
            {
                Debug.Log("Found a door item in a list!");
                IDs.Add(obj.GetComponentInChildren<DoorManager>().ID);
            }
            else if (obj.GetComponentInChildren<Elevator>() != null)
            {
                Debug.Log("Found a elevator item in a list!");
                IDs.Add(obj.GetComponentInChildren<Elevator>().ID);
            }
            else if (obj.GetComponentInChildren<Water>() != null)
            {
                Debug.Log("Found a water item in a list!");
                IDs.Add(obj.GetComponentInChildren<Water>().ID);
            }

            else if (obj.GetComponentInChildren<LevelLoader>() != null)
            {
                IDs.Add(obj.GetComponentInChildren<LevelLoader>().ID);
            }

            else if (obj.GetComponentInChildren<AirChannelManager>() != null)
            {
                IDs.Add(obj.GetComponentInChildren<AirChannelManager>().ID);
            }
        }
        return IDs;
    }
}
