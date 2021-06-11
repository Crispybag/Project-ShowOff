using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Linq;

public class SavingLevel : EditorWindow
{

    public string fileName = "empty";
    private string standardPathPrefix = "../Server/GameServer/GameServer/LevelFiles/";
    [HideInInspector] public string filePath = "";

    private int currentID = 0;


    [MenuItem("Window/SaveLevel")]
    public static void ShowWindow()
    {
        GetWindow<SavingLevel>("Save Level");

    }

    private void OnGUI()
    {
        fileName = EditorGUILayout.TextField("FileName", fileName);

        if (GUILayout.Button("Save level"))
        {
            Debug.Log("Saving level...");
            SaveLevel();
        }
    }


    public void SaveLevel()
    {
        //give everything ID's
        currentID = 0;
        GameObject[] interactables = GameObject.FindGameObjectsWithTag("Interactable");
        foreach(GameObject interactable in interactables)
        {
            if (interactable.GetComponentInChildren<PuzzleFactory>() != null)
            {
                interactable.GetComponentInChildren<PuzzleFactory>().ID = currentID;
                SetDirty(interactable.GetComponentInChildren<PuzzleFactory>());
            }
            else if (interactable.GetComponentInChildren<DoorManager>() != null)
            {
                interactable.GetComponentInChildren<DoorManager>().ID = currentID;
                SetDirty(interactable.GetComponentInChildren<DoorManager>());
            }
            else if (interactable.GetComponentInChildren<Elevator>() != null)
            {
                interactable.GetComponentInChildren<Elevator>().ID = currentID;
                SetDirty(interactable.GetComponentInChildren<Elevator>());
            }
            else if (interactable.GetComponentInChildren<BoxMovement>() != null)
            {
                interactable.GetComponentInChildren<BoxMovement>().ID = currentID;
                SetDirty(interactable.GetComponentInChildren<BoxMovement>());
            }
            else if (interactable.GetComponentInChildren<Dialogue>() != null)
            {
                interactable.GetComponent<Dialogue>().ID = currentID;
                SetDirty(interactable.GetComponent<Dialogue>());
            }
            else if (interactable.GetComponent<Water>() != null)
            {
                interactable.GetComponent<Water>().ID = currentID;
                SetDirty(interactable.GetComponent<Water>());
            }

            else if (interactable.GetComponentInChildren<LevelLoader>() != null)
            {
                interactable.GetComponentInChildren<LevelLoader>().ID = currentID;
                SetDirty(interactable.GetComponentInChildren<LevelLoader>());
            }

            else if (interactable.GetComponentInChildren<AirChannelManager>() != null)
            {
                interactable.GetComponentInChildren<AirChannelManager>().ID = currentID;
                SetDirty(interactable.GetComponentInChildren<AirChannelManager>());
            }


            currentID++;

        }
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
        Debug.Log("Saved level!");
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
