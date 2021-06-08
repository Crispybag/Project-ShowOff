using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IWWater : InformationWriter
{
    public override void StartSave()
    {
        base.StartSave();
    }

    public override void WriteAllInformation()
    {
        base.WriteAllInformation();
        base.AddToInformation(GetComponent<Water>().ID);
        base.AddToInformation(createList(this.gameObject.GetComponent<Water>().conditions));
        base.AddToInformation(createWater());
    }

    private List<Vector3> createWater()
    {
        List<Vector3> water = new List<Vector3>();
        for(int i = 0; i < this.transform.childCount; i++)
        {
            water.Add(this.transform.GetChild(i).transform.position);
        }
        return water;
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
        }
        return IDs;
    }
}
