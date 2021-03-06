using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (Ezra) Writes necessary information about the main functions for all actuators
/// </summary>

public class IWActuator : InformationWriter
{
    public override void StartSave()
    {
        base.StartSave();
    }
    public override void WriteAllInformation()
    {
        base.WriteAllInformation();
        AddToInformation(GetComponent<Actuators>().ID);
        AddToInformation(createList(this.gameObject.GetComponent<Actuators>().interactables));
    }

    private List<int> createList(List<GameObject> gameobjects)
    {
        List<int> IDs = new List<int>();
        foreach(GameObject obj in gameobjects)
        {
            if (obj.GetComponentInChildren<Actuators>() != null)
            {
                Debug.Log("Found a puzzle factory item in a list!");
                IDs.Add(obj.GetComponentInChildren<Actuators>().ID);
            }
            else if (obj.GetComponentInChildren<DoorManager>() != null)
            {
                Debug.Log("Found a door item in a list!");
                IDs.Add(obj.GetComponentInChildren<DoorManager>().ID);
            }
            else if (obj.GetComponentInChildren<Elevator>() != null)
            {
                Debug.Log("Found a elevator item in a list!");
                //IDs.Add(obj.GetComponentInChildren<Elevator>().ID);
                Elevator[] elevators = obj.GetComponentsInChildren<Elevator>();
                foreach ( Elevator elevator in elevators)
                {
                    IDs.Add(elevator.ID);
                }
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
