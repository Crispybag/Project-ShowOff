using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InformationWriterDoor : InformationWriter
{
    // Start is called before the first frame update
    public override void StartSave()
    {
        base.StartSave();
    }

    public override void WriteAllInformation()
    {
        base.WriteAllInformation();
        currentID++;
        base.AddToInformation(GetComponent<DoorManager>().ID);
        base.AddToInformation(createList(this.gameObject.GetComponent<DoorManager>().conditions));
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
        }
        return IDs;
    }


}
