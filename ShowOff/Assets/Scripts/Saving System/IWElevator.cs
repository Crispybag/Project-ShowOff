using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// (Ezra) Writes necessary information about the elevator mechanic
/// </summary>
public class IWElevator : InformationWriter
{
    // Start is called before the first frame update
    public override void StartSave()
    {
        base.StartSave();
    }

    public override void WriteAllInformation()
    {
        base.WriteAllInformation();
        AddToInformation(this.GetComponent<Elevator>().ID);
        AddToInformation(CreatePointList());
    }

    private List<int> CreatePointList()
    {
        List<int> points = new List<int>();
        foreach(GameObject point in this.gameObject.GetComponent<Elevator>().elevatorPoints)
        {
            points.Add((int)point.transform.position.x);
            points.Add((int)point.transform.position.y);
            points.Add((int)point.transform.position.z);
        }
        return points;
    }

}
