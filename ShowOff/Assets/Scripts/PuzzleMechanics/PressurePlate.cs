using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;

public class PressurePlate : PuzzleFactory
{
    //AUTHOR:
    //SHORT DISCRIPTION:

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------


    //----------------------- private ------------------------

    [SerializeField] private GameObject mesh;
    [SerializeField] private Material activatedMat;
    [SerializeField] private Material deactivatedMat;

    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================
    private void Start()
    {
        serviceLocator.interactableList.Add(ID, this.gameObject);
        this.gameObject.GetComponent<MeshRenderer>().material = deactivatedMat;
    }


    public void UpdateActuator(bool isActive)
    {
        gameObject.GetComponent<PlaySingleSound>().PlaySoundOnce();
        gameObject.transform.parent.GetComponentInChildren<Animator>().SetBool("activated", isActive);
        if (isActive)
        {
            mesh.GetComponent<MeshRenderer>().material = activatedMat;
        }
        else
        {
            mesh.GetComponent<MeshRenderer>().material = deactivatedMat;
        }
    }
}
