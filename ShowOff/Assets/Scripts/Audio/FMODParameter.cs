using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (Leo) Contains fmod paramater
/// </summary>

[System.Serializable]
public class FMODParameter
{
    [FMODUnity.EventRef]
    public string eventPath;

    public string parameterName;
    [Range(0f, 1f)]
    public float value;
}
