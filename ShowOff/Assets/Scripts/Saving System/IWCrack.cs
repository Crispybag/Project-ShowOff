using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (Ezra) Writes necessary information about the crack mechanic
/// </summary>

public class IWCrack : InformationWriter
{
    public override void StartSave()
    {
        base.StartSave();
    }

    public override void WriteAllInformation()
    {
        base.WriteAllInformation();
        AddToInformation(GetComponent<Crack>().ID);
        AddToInformation(createList(GetComponent<Crack>().interactables));
    }

    private List<int> createList(List<GameObject> gameobjects)
    {
        List<int> IDs = new List<int>();
        foreach (GameObject obj in gameobjects)
        {
            if (obj.GetComponentInChildren<Actuators>() != null)
            {
                IDs.Add(obj.GetComponentInChildren<Actuators>().ID);
            }
            else if (obj.GetComponentInChildren<DoorManager>() != null)
            {
                IDs.Add(obj.GetComponentInChildren<DoorManager>().ID);
            }
            else if (obj.GetComponentInChildren<Elevator>() != null)
            {
                IDs.Add(obj.GetComponentInChildren<Elevator>().ID);
            }
            else if (obj.GetComponentInChildren<Water>() != null)
            {
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
