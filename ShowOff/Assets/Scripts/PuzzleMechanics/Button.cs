using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;

/// <summary>
/// (Ezra) Contains logic about the button mechanic
/// </summary>

public class Button : Actuators
{
    private void Start()
    {
        serviceLocator.interactableList.Add(ID, this.gameObject);
    }

    public void UpdateActuator(bool isActive)
    {
        gameObject.GetComponent<PlaySingleSound>().PlaySoundOnce();
    }
}
