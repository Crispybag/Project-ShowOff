using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;

public class ForceMovement : MonoBehaviour
{
    //AUTHOR: Leo Jansen
    //SHORT DISCRIPTION: Force based movement

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------


    //----------------------- private ------------------------
    private InputManager _inputManager;

    [SerializeField] private float _force = 1000;
    [SerializeField] private float _gravity = 3000;
    [SerializeField] private float _jumpForce = 300;
    private Rigidbody _rb;
    private bool _isFalling;
    private bool _isGrounded;
    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================

    private void Awake()
    {
        serviceLocator.AddToList("Player1", this.gameObject);
    }

    private void Start()
    {
        _inputManager = serviceLocator.GetFromList("InputManager").GetComponent<InputManager>();

        _rb = GetComponent<Rigidbody>();
        Debug.Assert(GetComponent<Rigidbody>() != null, "No rigidbody is attached to the player");
    }

    private void Update()
    {
        if (_isFalling)
        {
            _rb.AddForce(new Vector3(_inputManager.getHorizontalInput() * _force, -_gravity, 0) * Time.deltaTime);
            _rb.AddForce(new Vector3(0, 0, _inputManager.getVerticalInput() * _force) * Time.deltaTime);
        }
        else
        {
            _rb.AddForce(new Vector3(_inputManager.getHorizontalInput() * _force, 0, 0) * Time.deltaTime);
            _rb.AddForce(new Vector3(0,0, _inputManager.getVerticalInput() * _force) * Time.deltaTime);
            if (_inputManager.GetActionDown(InputManager.Action.ACT0))
            {
                _rb.AddForce(new Vector3(0, _jumpForce, 0));
            }
        }
    }

    private void FixedUpdate()
    {
        checkForGrounded();
    }

    private void OnDrawGizmos()
    {
        Vector3 boxBoundaries = new Vector3(1f, 0.1f, 1f);
        Vector3 center = new Vector3(0.0f, -0.5f, 0.0f) + transform.position;
        if (_isFalling)
        {
            Gizmos.color = new Color(0, 0, 1);
            Gizmos.DrawWireCube(center, boxBoundaries);
        }
        else
        {
            Gizmos.color = new Color(1, 0, 0);
            Gizmos.DrawWireCube(center, boxBoundaries);
        }
    }
    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================
    private void checkForGrounded()
    {

        Vector3 boxBoundaries = new Vector3(0.5f, 0.0f, 0.5f);
        Vector3 center = new Vector3(0.0f, -0.4f, 0.0f) + transform.position;
        Quaternion orientation = Quaternion.Euler(0, 0, 0);

        if (Physics.BoxCast(center, boxBoundaries, -transform.up, orientation, 0.2f))
        {
            _isFalling = false;
        }
        else
        {
            _isFalling = true;
        }
    }
}

