using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SavingLevelEditor : Editor
{
    [CustomEditor(typeof(SavingLevel))]
    public class ObjectBuilderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Save level"))
            {
                try
                {
                    Debug.Log("Saving level!");
                    FindObjectOfType<SavingLevel>().SaveLevel();
                }
                catch
                {

                }
            }
        }
    }
}
