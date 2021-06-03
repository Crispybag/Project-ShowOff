using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Linq;

public class SavingLevel : MonoBehaviour
{

    public string fileName = "empty";
    private string standardPathPrefix = "../Server/GameServer/GameServer/LevelFiles/";
    [HideInInspector] public string filePath = "";

    private int currentID = 0;

    public void SaveLevel()
    {
        //give everything ID's
        currentID = 0;
        GameObject[] interactables = GameObject.FindGameObjectsWithTag("Interactable");
        foreach(GameObject interactable in interactables)
        {
            if (interactable.GetComponentInChildren<PuzzleFactory>() != null)
            {
                Debug.Log("Found a puzzle factory, setting a new ID: " + currentID + "!");
                interactable.GetComponentInChildren<PuzzleFactory>().ID = currentID;
                SetDirty(interactable.GetComponentInChildren<PuzzleFactory>());
            }
            else if (interactable.GetComponentInChildren<DoorManager>() != null)
            {
                Debug.Log("Found a door, setting a new ID: " + currentID + "!");
                interactable.GetComponentInChildren<DoorManager>().ID = currentID;
                SetDirty(interactable.GetComponentInChildren<DoorManager>());
            }
            else if (interactable.GetComponentInChildren<Elevator>() != null)
            {
                Debug.Log("Found a elevator, setting a new ID: " + currentID + "!");
                interactable.GetComponentInChildren<Elevator>().ID = currentID;
                SetDirty(interactable.GetComponentInChildren<Elevator>());
            }
            else if (interactable.GetComponentInChildren<BoxMovement>() != null)
            {
                Debug.Log("Found a box, setting a new ID: " + currentID + "!");
                interactable.GetComponentInChildren<BoxMovement>().ID = currentID;
                SetDirty(interactable.GetComponentInChildren<BoxMovement>());
            }
            else if (interactable.GetComponentInChildren<Dialogue>() != null)
            {
                Debug.Log("Found a dialogue, setting a new ID: " + currentID + "!");
                interactable.GetComponent<Dialogue>().ID = currentID;
                SetDirty(interactable.GetComponent<Dialogue>());
            }
            currentID++;

        }
        Debug.Log("Finished setting ID's");
        //create lists for actuators from given id's


        //save information
        filePath = standardPathPrefix + fileName + ".txt";
        //clear the txt
        File.WriteAllText(filePath, string.Empty);
        
        InformationWriter[] writers = FindObjectsOfType(typeof(InformationWriter)) as InformationWriter[];
        
        foreach(InformationWriter writer in writers)
        {
            try
            {
                SetDirty(writer);
                writer.fileName = filePath;
                writer.StartSave();
            }
            catch
            {
                Debug.LogError("Someting went wrong when trying to write to file : " + writer.gameObject.name);
            }
        }
    }

    public static void SetDirty(Object o)
    {
        try
        {
            EditorUtility.SetDirty(o);

            if (o is GameObject)
            {
                var scene = ((GameObject)o).scene;
                EditorSceneManager.MarkSceneDirty(scene);
            }

            if (o is Component)
            {
                var go = ((Component)o).gameObject;
                var scene = go.scene;
                EditorSceneManager.MarkSceneDirty(scene);
            }
        }
        catch
        {
            Debug.LogError("Something went wrong when trying to make something dirty" + o.name);
        }
    }
}
