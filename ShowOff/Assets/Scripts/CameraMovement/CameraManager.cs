using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;

public class CameraManager : MonoBehaviour
{
    //AUTHOR: Ezra
    //SHORT DISCRIPTION: Moves camera, either to position or following player.

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------



    //----------------------- private ------------------------

    public CameraState cameraState = CameraState.FOLLOW;
    [SerializeField]
    private GameObject _cameraGameObject;    
    [SerializeField]
    private GameObject _playerCameraGameObject;


    private GameObject _startCameraData;
    private GameObject _endCameraData;

    private bool isLerping = false;
    private bool isFollowingPlayer = false;

    private GameObject _newCameraPosition;

    private float startTime;
    private float _speed = 0.5f;

    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================
    private void Awake()
    {
        serviceLocator.AddToList("CameraManager", gameObject);
    }

    private void Start()
    {
        SetFollow(_speed);
    }

    private void Update()
    {
        if (cameraState == CameraState.FOLLOW)
        {
            _startCameraData = _cameraGameObject;
            _endCameraData = _playerCameraGameObject;
        }
        if (cameraState == CameraState.LOCKED)
        {
            _startCameraData = _cameraGameObject;
            _endCameraData = _newCameraPosition;
        }

        LerpToPosition();
    }


    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    public void SetPosition(GameObject pCameraData, float pSpeed, bool pisFollowingPlayer)
    {
        _speed = pSpeed;
        startTime = Time.time;
        _newCameraPosition = pCameraData;
        cameraState = CameraState.LOCKED;
        isLerping = true;
        isFollowingPlayer = pisFollowingPlayer;
    }

    public void SetFollow(float pSpeed)
    {
        _speed = pSpeed;
        startTime = Time.time;
        cameraState = CameraState.FOLLOW;
        isLerping = true;
    }



    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================

    private void LerpToPosition()
    {
        if (isLerping)
        {
            _cameraGameObject.transform.position = Vector3.Slerp(_startCameraData.transform.position, _endCameraData.transform.position, _speed * (Time.time - startTime));
            _cameraGameObject.transform.rotation = Quaternion.Slerp(_startCameraData.transform.rotation, _endCameraData.transform.rotation, _speed * (Time.time - startTime));
            if (cameraState != CameraState.FOLLOW && !isFollowingPlayer && _cameraGameObject.transform.position == _endCameraData.transform.position && _cameraGameObject.transform.rotation == _endCameraData.transform.rotation)
            {
                isLerping = false;
            }
        }
    }

    public enum CameraState
    {
        LOCKED,
        FOLLOW
    }

}
