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

    private List<GameObject> _currentObjects = new List<GameObject>();
    [SerializeField] private Material mat1;
    [SerializeField] private Material mat2;

    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================
    private void Start()
    {
        serviceLocator.interactableList.Add(ID, this.gameObject);
    }

    private void Update()
    {

    }

    public void UpdateActuator(bool isActive)
    {
        gameObject.GetComponent<PlaySingleSound>().PlaySoundOnce();

    }
}
