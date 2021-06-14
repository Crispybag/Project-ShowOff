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
/*        if (isActive)
        {
            GetComponent<MeshRenderer>().material = mat2;
            isActuated = true;
            ToggleMechanics();
        }
        else
        {
            GetComponent<MeshRenderer>().material = mat1;
            isActuated = false;
            ToggleMechanics();
        }*/
    }
}
