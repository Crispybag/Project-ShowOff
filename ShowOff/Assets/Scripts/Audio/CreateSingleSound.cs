using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;

/// <summary>
/// (Leo) Creates a singleton sound
/// </summary>

public class CreateSingleSound : MonoBehaviour
{
    [FMODUnity.EventRef]
    [SerializeField] private string eventPath;

    private void Start()
    {
        AudioManager am = serviceLocator.GetFromList("AudioManager").GetComponent<AudioManager>();
        am.AddToList(eventPath, eventPath);
    }
}
