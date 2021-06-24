using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;

/// <summary>
/// (Ezra) Contains logic about the lever mechanic
/// </summary>

public class Lever : Actuators
{

    [FMODUnity.EventRef]
    public string eventPath;

    private void Start()
    {
        serviceLocator.interactableList.Add(ID, this.gameObject);
    }


    public void SetActivatedLever(bool isActive)
    {
        isActuated = isActive;
        serviceLocator.GetFromList("AudioManager").GetComponent<AudioManager>().playSound(eventPath, this.gameObject);
    }

}