using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;
using FMODUnity;

public class CameraTrigger : MonoBehaviour
{
    //AUTHOR: Ezra
    //SHORT DISCRIPTION: Triggers to switch camera to something.

    private CameraManager _cameraManager;
    [SerializeField] private GameObject _cameraPosition;
    [SerializeField] private float _transitionSpeed;
    [SerializeField] private bool _isFollowingPlayer;

    private void Start()
    {
        _cameraManager = serviceLocator.GetFromList("CameraManager").GetComponent<CameraManager>();
        GetComponent<Collider>().enabled = false;
    }

    bool firstRun = true;
    private void Update()
    {
        if (firstRun) { GetComponent<Collider>().enabled = true; firstRun = false; }
    }

    public void SetToPosition()
    {
            _cameraManager.SetPosition(_cameraPosition, _transitionSpeed, _isFollowingPlayer);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().ClientName)
        {
            SetToPosition();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().ClientName)
        {
            _cameraManager.SetFollow(_transitionSpeed);
        }
    }

}