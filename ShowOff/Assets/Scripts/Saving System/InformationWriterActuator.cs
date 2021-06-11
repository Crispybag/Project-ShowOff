using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationWriterActuator : InformationWriter
{
    public override void StartSave()
    {
        base.StartSave();
    }
    public override void WriteAllInformation()
    {
        base.WriteAllInformation();
        AddToInformation(GetComponent<PuzzleFactory>().ID);
        AddToInformation(createList(this.gameObject.GetComponent<PuzzleFactory>().interactables));
    }

    private List<int> createList(List<GameObject> gameobjects)
    {
        List<int> IDs = new List<int>();
        foreach(GameObject obj in gameobjects)
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
