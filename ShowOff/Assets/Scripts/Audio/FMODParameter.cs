using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class FMODParameter
{
    [FMODUnity.EventRef]
    public string eventPath;

    public string parameterName;
    [Range(0f, 1f)]
    public float value;
}
