using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    //AUTHOR: Ezra
    //SHORT DISCRIPTION: Triggers to switch camera to something.

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------

    //----------------------- private ------------------------

    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private GameObject _cameraPosition;
    [SerializeField] private float _transitionSpeed;
    [SerializeField] private bool _isFollowingPlayer;

    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _cameraManager.SetPosition(_cameraPosition, _transitionSpeed, _isFollowingPlayer);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _cameraManager.SetFollow(_transitionSpeed);
        }
    }

}