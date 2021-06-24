using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;

/// <summary>
/// (Leo) Plays sound once
/// </summary>

public class PlaySingleSound : MonoBehaviour
{
    [FMODUnity.EventRef]
    [SerializeField] private string eventPath;
    AudioManager am;

    [FMODUnity.EventRef]
    [SerializeField] private List<string> eventpaths;

    private void Start()
    {
        am = serviceLocator.GetFromList("AudioManager").GetComponent<AudioManager>();
    }

    public void PlaySoundOnce()
    {
        am.playSound(eventPath, gameObject);
    }


    public void PlaysoundFromlist(int value)
    {
        am.playSound(eventpaths[value], gameObject);
    }
}


