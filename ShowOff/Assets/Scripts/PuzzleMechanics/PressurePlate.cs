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

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================

/*    private void OnTriggerEnter(Collider other)
    {
        foreach(string tag in _collisionTags)
        {
            if(other.tag == tag)
            {
                _currentObjects.Add(other.gameObject);
                UpdateActuator();
                break;
            }
        }
    }*/

/*    private void OnTriggerExit(Collider other)
    {
        foreach (string tag in _collisionTags)
        {
            if (other.tag == tag)
            {
                _currentObjects.Remove(other.gameObject);
                UpdateActuator();
                break;
            }
        }
    }*/

    public void UpdateActuator(bool isActive)
    {
        if(isActive)
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
        }
    }
}
