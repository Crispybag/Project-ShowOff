using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IWLevelLoader : InformationWriter
{
    // Start is called before the first frame update
    [SerializeField] private string newLevelName;
    
    public override void WriteAllInformation()
    {
        if (isNameValid(newLevelName))
        {
            base.WriteAllInformation();
            AddToInformation(newLevelName);
            AddToInformation(GetComponent<LevelLoader>().ID);
            AddToInformation(createList(GetComponent<LevelLoader>().conditions));
        }
    }

    private bool isNameValid(string pName)
    {
        foreach(char letter in pName)
        {
            if (letter == '(' || letter == ')' || letter ==' ' || pName.Length == 0)
            {
                Debug.LogError("Name of the level loader contains invalid characters (a space or ()) or has a name that has 0 length, the level loader has been skipped, please reconsider a new name");
                return false;
            }
        }
        return true;
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
