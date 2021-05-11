using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : ActuatorFactoy
{
    //AUTHOR:
    //SHORT DISCRIPTION:

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------

    //----------------------- private ------------------------

    private List<GameObject> _currentObjects = new List<GameObject>();
    [SerializeField] private string[] _collisionTags;
    [SerializeField] private Material mat1;
    [SerializeField] private Material mat2;

    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================
    private void Start()
    {

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

    private void OnTriggerEnter(Collider other)
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
    }

    private void OnTriggerExit(Collider other)
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
    }

    private void UpdateActuator()
    {
        if(_currentObjects.Count > 0)
        {
            GetComponent<MeshRenderer>().material = mat2;
            isActuated = true;
        }
        else 
        {
            GetComponent<MeshRenderer>().material = mat1;
            isActuated = false; 
        }
    }


}
