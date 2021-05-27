using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;

public class Lever : PuzzleFactory
{

    //AUTHOR: Ezra
    //SHORT DISCRIPTION:

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------



    //----------------------- private ------------------------

    [SerializeField] private Material _mat1;
    [SerializeField] private Material _mat2;
    private InputManager _inputManager;

    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================
    private void Start()
    {
        serviceLocator.interactableList.Add(ID, this.gameObject);
        this.gameObject.GetComponent<MeshRenderer>().material = _mat1;
    }

    private void Update()
    {
    }

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    public void SetActivatedLever(bool isActive)
    {
        isActuated = isActive;
        setMaterial();
    }

    private void setMaterial()
    {
        if (isActuated)
        {
            this.gameObject.GetComponent<MeshRenderer>().material = _mat2;
        }
        else
        {
            this.gameObject.GetComponent<MeshRenderer>().material = _mat1;
        }
    }



    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================

}