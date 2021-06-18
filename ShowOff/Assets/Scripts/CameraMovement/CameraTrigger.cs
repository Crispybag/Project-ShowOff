using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;
using FMODUnity;

public class CameraTrigger : MonoBehaviour
{
    //AUTHOR: Ezra
    //SHORT DISCRIPTION: Triggers to switch camera to something.

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------

    //----------------------- private ------------------------

    private CameraManager _cameraManager;
    [SerializeField] private GameObject _cameraPosition;
    [SerializeField] private float _transitionSpeed;
    [SerializeField] private bool _isFollowingPlayer;

    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================

    private void Start()
    {
        _cameraManager = serviceLocator.GetFromList("CameraManager").GetComponent<CameraManager>();
    }

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================
    public void SetToPosition()
    {
            _cameraManager.SetPosition(_cameraPosition, _transitionSpeed, _isFollowingPlayer);

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Got an enter!");
        if (other.tag == serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().ClientName)
        {
            SetToPosition();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Got and exit!");
        if (other.tag == serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().ClientName)
        {
            _cameraManager.SetFollow(_transitionSpeed);
        }
    }

}