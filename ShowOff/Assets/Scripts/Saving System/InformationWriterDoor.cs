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
                IDs.Add(obj.GetComponentInChildren<PuzzleFactory>().ID);
            }
            else if (obj.GetComponentInChildren<DoorManager>() != null)
            {
                IDs.Add(obj.GetComponentInChildren<DoorManager>().ID);
            }
            else if (obj.GetComponentInChildren<Elevator>() != null)
            {
                IDs.Add(obj.GetComponentInChildren<Elevator>().ID);
            }
            else if (obj.GetComponentInChildren<LevelLoader>() != null)
            {
                IDs.Add(obj.GetComponentInChildren<LevelLoader>().ID);
            }
        }
        return IDs;
    }


}
