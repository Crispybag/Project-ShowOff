using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;

public class Button : PuzzleFactory
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
