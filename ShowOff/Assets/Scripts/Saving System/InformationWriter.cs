using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class InformationWriter : MonoBehaviour
{
    //AUTHOR: Leo Jansen
    //SHORT DISCRIPTION: Base class that you can attach to an object you want to write in a specific scene

    #region variables
    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //----------------------- public ------------------------

    [HideInInspector] public int currentID;

    //----------------------- private ------------------------
    //the object index of the object
    [SerializeField] private int _objectIndex;

    //the path of the object
     [HideInInspector] public string fileName = "empty";
    //[HideInInspector]public string standardPathPrefix = "../Server/GameServer/GameServer/LevelFiles/";

    //string that all information will be written to
    private string informationString;
    #endregion

    #region start/update
    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================

    // Update is called once per frame
    private void Awake()
    {
        //fileName = standardPathPrefix + fileName + ".txt";
        //clearData();
    }
    public virtual void Start()
    {

    }

    public virtual void StartSave()
    {
        informationString = "";
        WriteAllInformation();
        saveData();
    }
    #endregion

    #region public tool functions
    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================
    //tools for children to append their own information
    protected void AddToInformation(float pInfo)
    {
        informationString += (int)pInfo + " ";
    }
    protected void AddToInformation(Vector3 pInfo)
    {
        informationString += (int)pInfo.x + " ";
        informationString += (int)pInfo.y + " ";
        informationString += (int)pInfo.z + " ";
    }

    protected void AddToInformation(List<int> pInfo)
    {
        informationString += "( ";
        foreach (int element in pInfo)
        {
            informationString += element + " ";
        }
        informationString += ") ";
    }

    protected void AddToInformation(List<Vector3> pInfo)
    {
        informationString += "( ";
        foreach (Vector3 element in pInfo)
        {
            informationString += element.x + " " + element.y + " " + element.z + " ";
        }
        informationString += ") ";
    }


    protected void AddToInformation(string pInfo)
    {
        informationString += pInfo + " ";
    }


    //Main function to be overwritten by child classes if you want to add your own information. Make sure to not forget to add base.WriteAllinformation(). Because that can really mess up things
    public virtual void WriteAllInformation()
    {
        AddToInformation(_objectIndex);
        AddToInformation(gameObject.transform.position);
        AddToInformation(gameObject.transform.localScale);
    }
    #endregion

    #region private tool functions
    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================
    /// <summary>
    /// main save function, this function writes all information to the text file
    /// </summary>
    private void saveData()
    {
        if (!File.Exists(fileName))
        {
            File.Create(fileName);
        }

            //create a lines list that reads all lines from a file
            List<string> lines;
        lines = File.ReadAllLines(fileName).ToList();

        //write the constructed string
        lines.Add(informationString);
        //write all lines to the file
        File.WriteAllLines(fileName, lines);
    }

    //clears all data from the file to be overwritten later
    private void clearData()
    {
        File.WriteAllText(fileName, string.Empty);
    }
    #endregion 


}
