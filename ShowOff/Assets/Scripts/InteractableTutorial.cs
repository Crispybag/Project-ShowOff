using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;

public class InteractableTutorial : MonoBehaviour
{
    //AUTHOR: Ezra 
    //SHORT DISCRIPTION: Shows when something is interactable, with a popup

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------


    //----------------------- private ------------------------

    [SerializeField] private GameObject _object;
    [SerializeField] private float _visibleRadius;
    private Camera _cam;

    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================
    private void Start()
    {
        _object.GetComponent<MeshRenderer>().enabled = false;
        _cam = Camera.main;
    }

    private void LateUpdate()
    {
        VisiblityRange();
        if (_object.GetComponent<MeshRenderer>().enabled)
        {
            transform.LookAt(transform.position + _cam.transform.rotation * Vector3.forward, _cam.transform.rotation * Vector3.up);
        }
    }

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================

    private void VisiblityRange()
    {
        if(Vector3.Distance(this.transform.position, serviceLocator.GetFromList("Player1").transform.position) < _visibleRadius)
        {
            _object.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            _object.GetComponent<MeshRenderer>().enabled = false;
        }
    }

}