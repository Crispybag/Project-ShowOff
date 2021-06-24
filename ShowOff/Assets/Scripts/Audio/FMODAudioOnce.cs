using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;

/// <summary>
/// (Leo) Plays a singleton sound
/// </summary>

public class FMODAudioOnce : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string _eventPath;
   // [SerializeField] private string _eventName;

    void Start()
    {
        AudioManager manager = serviceLocator.GetFromList("AudioManager").GetComponent<AudioManager>();
        manager.AddToList(_eventPath, _eventPath);
        manager.PlayFromList(_eventPath);
    }

}
