using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    //AUTHOR: Leo Jansen    
    //SHORT DISCRIPTION: Base Movement for most things

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------


    //----------------------- private ------------------------
    protected float _travelTime;

    //determines whether an object can fall
    protected bool canFall;
    protected bool canMove;
    protected Vector3 _currentPosition;
    private Vector3 _targetPosition;
    
    //int layers of terrain and movable
    private int terrainLayer;
    private int movableLayer;


    private float timer;
    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================
    protected virtual void Start()
    {
        _currentPosition = transform.position;
        _targetPosition = transform.position;
    }
    
    protected virtual void Update()
    {
        timer += Time.deltaTime;
        float ratio = timer / _travelTime;
        transform.position = Vector3.Lerp(_currentPosition, _targetPosition, ratio);
        checkForMovement();
    }

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================
    /// <summary>
    /// Checks if the object can move to a certain direction and then sets the direction to that 
    /// </summary>
    /// <param name="pDirection"> the place the player wants to move to</param>
    protected void moveToTile(Vector3 pDirection)
    {
        if (!wallCheck(_currentPosition + pDirection, _currentPosition) && canMove)
        {
            _targetPosition = pDirection + _currentPosition;
            timer = 0f;
        }
    }

    /// <summary>
    /// checks if the object is currently moving
    /// </summary>
    protected void checkForMovement()
    {
        if ((_targetPosition - transform.position).magnitude < 0.001f)
        {
            canMove = true;
            transform.position = _targetPosition;
            _currentPosition = transform.position;
        }
        else canMove = false;

    }
    /// <summary>
    /// checks for a wall and any moving objects that try to move further
    /// </summary>
    /// <param name="pTargetPosition"> the position the player is headed for </param>
    /// <param name="pCurrentPosition"> the current position of the player</param>
    /// <returns></returns>
    protected bool wallCheck(Vector3 pTargetPosition, Vector3 pCurrentPosition)
    {
        RaycastHit hit;
        Vector3 moveDirection = pTargetPosition - pCurrentPosition;


        if (Physics.Raycast(pCurrentPosition, pTargetPosition - pCurrentPosition, out hit, 1) )
        {
            //if it finds terrain
            if (hit.collider.gameObject.layer == terrainLayer) { return true; }
            
            //recursion check if object is movable (pushable box)
            else if (hit.collider.gameObject.layer == movableLayer)
            {
                try
                {
                    return hit.collider.gameObject.GetComponent<Movement>().wallCheck(pTargetPosition, moveDirection);
                }

                catch
                {
                    Debug.LogError("Movable Layer does not have a movement component");
                    return true;
                }
            }

            //if it found something random
            else { return false; }
        }

        //when it doesnt find anything
        else { return false; }
    }
}
