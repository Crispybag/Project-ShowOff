using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;

public abstract class Movement : MonoBehaviour
{
    //AUTHOR:
    //SHORT DISCRIPTION:

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------

    public Vector3 toBePosition;     //position it will be in after lerp

    public Vector3 leftDirection;
    public Vector3 rightDirection;

    //----------------------- private ------------------------

    private int terrainLayer = 6;
    private int movableLayer = 7;

    protected Vector3 _currentPosition;
    private Vector3 _targetPosition;
    
    protected Vector3 _currentRotation;
    private Vector3 _targetRotation;

    protected float _travelTime = 0.1f;
    private float timer;

    public bool canMove = true;

    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================
    protected virtual void Start()
    {
        toBePosition = transform.position;
        _currentPosition = transform.position;
        _targetPosition = transform.position;
        _currentRotation = transform.position;
        _targetRotation = transform.position;
    }

    protected virtual void Update()
    {
        timer += Time.deltaTime;
        float ratio = timer / _travelTime;
        transform.position = Vector3.Lerp(_currentPosition, _targetPosition, ratio);
        transform.transform.LookAt(Vector3.Lerp(_currentRotation, _targetRotation, ratio));


        checkForMovement();
        checkForFalling();
    }


    public void checkForFalling()
    {
        if (canMove)
        {
            if (Physics.Raycast(_currentPosition, -Vector3.up, out RaycastHit hit, 1))
            {
                if (hit.collider.gameObject.tag != "AirChannel" && hit.collider.gameObject.layer != terrainLayer)
                {
                    moveToTile(-Vector3.up);
                }
            }
            else
            {
                moveToTile(-Vector3.up);
            }
        }
    }

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    public void moveToTile(Vector3 pDirection)
    {
        if (canMove)
        {
            if (!wallCheck(_currentPosition + pDirection, _currentPosition))
            {
                if (!WallFarCheck(_currentPosition + pDirection, _currentPosition))
                {
                    Debug.Log(this.gameObject.ToString() + "moving!");
                    rotateToTile(pDirection);
                    _targetPosition = pDirection + _currentPosition;
                    toBePosition = _targetPosition;
                    timer = 0f;
                }
            }
        }
    }

    public void rotateToTile(Vector3 pDirection)
    {
        _targetRotation = this.transform.position - pDirection;
        _targetRotation.x = 0;
        _targetRotation.z = 0;
/*        this.gameObject.transform.LookAt(this.transform.position + pDirection);
        Vector3 lookpos = this.transform.position + pDirection;
        float boop = Mathf.Atan2(lookpos.z - this.transform.position.z, lookpos.x - this.transform.position.x) * 180 / Mathf.PI;*/
    }

    protected void checkForMovement()
    {
        if ((_targetPosition - transform.position).magnitude < 0.001f)
        {
            canMove = true;
            transform.position = _targetPosition;
            _currentPosition = transform.position;
        }
        else
        {
            canMove = false;
        }
    }
    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================

    virtual public bool wallCheck(Vector3 pTargetPosition, Vector3 pCurrentPosition)
    {
        Vector3 moveDirection = pTargetPosition - pCurrentPosition;
         leftDirection = getLeftFromDirection(moveDirection);
         rightDirection = getRightFromDirection(moveDirection);

        if (Physics.Raycast(pCurrentPosition, moveDirection, out RaycastHit frontHit, 1))
        {
            if (frontHit.collider.gameObject.layer == terrainLayer) 
            {
                return true; 
            }
            else if (frontHit.collider.gameObject.layer == movableLayer)
            {
                return frontHit.collider.gameObject.GetComponent<Movement>().wallCheck(pTargetPosition + moveDirection, pTargetPosition);
            }
            else { return false; }
        }

        else if (Physics.Raycast(pCurrentPosition, leftDirection, out RaycastHit leftHit, 1))
        {
             if (leftHit.collider.gameObject.layer == movableLayer)
            {
                Debug.Log(this.gameObject.ToString() + "left moveable hit!");
                if (pTargetPosition == leftHit.collider.gameObject.GetComponent<Movement>().toBePosition)
                {
                    Debug.Log(this.gameObject.ToString() + "left doom hit!");
                    return true;
                }
                else if (pTargetPosition == leftHit.collider.gameObject.transform.position)
                {
                    Debug.Log(this.gameObject.ToString() + "left second doom hit!");
                    return true;
                }
                else { return false; }
            }
            else { return false; }
        }

        else if (Physics.Raycast(pCurrentPosition, rightDirection, out RaycastHit rightHit, 1))
        {
            if (rightHit.collider.gameObject.layer == movableLayer)
            {
                Debug.Log(this.gameObject.ToString() + "right moveable hit!");
                if (pTargetPosition == rightHit.collider.gameObject.GetComponent<Movement>().toBePosition)
                {
                    Debug.Log(this.gameObject.ToString() + "right doom hit!");
                    return true;
                }
                else if (pTargetPosition == rightHit.collider.gameObject.transform.position)
                {
                    Debug.Log(this.gameObject.ToString() + "right second doom hit!");
                    return true;
                }
                else { return false; }
            }
            else { return false; }
        }

        else { return false; }
    }

    private Vector3 getLeftFromDirection(Vector3 pDirection)
    {
        Vector3 left = pDirection;
        if(left.x == 0)
        {
            left.x -= 1;
        }
        if(left.z == 0)
        {
            left.z -= 1;
        }
        return left;
    }

    private Vector3 getRightFromDirection(Vector3 pDirection)
    {
        Vector3 left = pDirection;
        if (left.x == 0)
        {
            left.x += 1;
        }
        if (left.z == 0)
        {
            left.z += 1;
        }
        return left;
    }

    public bool WallFarCheck(Vector3 pTargetPosition, Vector3 pCurrentPosition)
    {
        Vector3 moveDirection = pTargetPosition - pCurrentPosition;
        if (Physics.Raycast(pCurrentPosition, moveDirection, out RaycastHit hit, 2))
        {
            if (hit.collider.gameObject.layer == movableLayer)
            {
                if (pTargetPosition == hit.collider.gameObject.GetComponent<Movement>().toBePosition)
                {
                    hit.collider.gameObject.GetComponent<Movement>().moveToTile(moveDirection);
                }
                else if(pTargetPosition == hit.collider.gameObject.transform.position)
                {
                    hit.collider.gameObject.GetComponent<Movement>().moveToTile(moveDirection);
                }
            }
        }
        //no collisions found so clear to advance
        return false;
    }
    
    
    /*    public void WallFarCheck(Vector3 pTargetPosition, Vector3 pCurrentPosition)
    {
        Vector3 moveDirection = pTargetPosition - pCurrentPosition;
        if (Physics.Raycast(pCurrentPosition, moveDirection, out RaycastHit hit, 2))
        {
            if (hit.collider.gameObject.layer == movableLayer)
            {
                if (pTargetPosition == hit.collider.gameObject.GetComponent<Movement>().toBePosition)
                {
                    hit.collider.gameObject.GetComponent<Movement>().moveToTile(moveDirection);
                }
                else if(pTargetPosition == hit.collider.gameObject.transform.position)
                {
                    hit.collider.gameObject.GetComponent<Movement>().moveToTile(moveDirection);
                }
            }
        }
    }*/

}











/*public abstract class Movement : MonoBehaviour
{
    //AUTHOR: Leo Jansen    
    //SHORT DISCRIPTION: Base Movement for most things

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------

    [SerializeField] private LayerMask _collisionLayers;
    [SerializeField] private LayerMask _moveableLayers;
    [SerializeField] private LayerMask _airChannelLayer;


    //----------------------- private ------------------------
    protected float _travelTime = 0.1f;

    [SerializeField, Tooltip("Higher numbers mean lower weight")] public int weight = 2;

    //determines whether an object can fall
    public bool canFall = true;
    public bool canMove;
    public bool isGrounded = true;
    protected Vector3 _currentPosition;
    private Vector3 _targetPosition;

    //int layers of terrain and movable
    private int terrainLayer = 6;
    private int movableLayer = 7;

    public State currentState = State.GROUNDED;

    public int pushedAmount = 0;

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

        checkForFalling();
        checkForMovement();
    }


    //=========================================================================================
    //                              > Private Tool Functions <
    //=========================================================================================
    /// <summary>
    /// determines if the object should fall or not
    /// </summary>
    public void checkForFalling()
    {
        if (currentState != State.AIRING)
        {
            if (canFall && canMove)
            {
                bool isHitting = Physics.Raycast(_currentPosition, -Vector3.up, out RaycastHit hit, 1, _collisionLayers);
                if (isHitting == false)
                {
                    moveToTile(-Vector3.up);
                }
                else
                {
                    currentState = State.GROUNDED;
                }
            }
        }
    }

    public bool CheckForAirChannel()
    {
        return Physics.Raycast(_currentPosition, -Vector3.up, out RaycastHit hit, 1, _collisionLayers);
    }


    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================
    /// <summary>
    /// Checks if the object can move to a certain direction and then sets the direction to that 
    /// </summary>
    /// <param name="pDirection"> the place the player wants to move to</param>
    public void moveToTile(Vector3 pDirection)
    {
            if (canMove)
            {
                if (!wallCheck(_currentPosition + pDirection, _currentPosition) && canMove)
                {
                    _targetPosition = pDirection + _currentPosition;
                    timer = 0f;
                }
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
        else
        {
            if (currentState != State.AIRING)
            {
                currentState = State.MOVING;
            }
            canMove = false;
        }

    }
    /// <summary>
    /// checks for a wall and any moving objects that try to move further
    /// </summary>
    /// <param name="pTargetPosition"> the position the player is headed for </param>
    /// <param name="pCurrentPosition"> the current position of the player</param>
    /// <returns></returns>
    virtual public bool wallCheck(Vector3 pTargetPosition, Vector3 pCurrentPosition, int calls = 0)
    {
        if (Physics.Raycast(pCurrentPosition, pTargetPosition - pCurrentPosition, out RaycastHit hit, 1) )
        {
            //if it finds terrain
            if (hit.collider.gameObject.layer == terrainLayer) { return true; }
            
            //recursion check if object is movable (pushable box)
            else if (hit.collider.gameObject.layer == movableLayer)
            {
                calls += hit.collider.gameObject.GetComponent<Movement>().weight;
                if (calls > 100) { Debug.LogError("You are likely causing an infinity loop"); return false; }
                Vector3 moveDirection = pTargetPosition - pCurrentPosition;
                return hit.collider.gameObject.GetComponent<Movement>().wallCheck(pTargetPosition + moveDirection, pTargetPosition, calls);
            }
            //if it found something random
            else { return false; }
        }
        //when it doesnt find anything
        else { return false; }
    }



    /// <summary>
    /// Can turn on and off falling
    /// </summary>
    /// <param name="pCanFall"></param>
    public void setFalling(bool pCanFall)
    {
        canFall = pCanFall;
    }

    public enum State
    {
        GROUNDED,
        MOVING,
        AIRING
    }

}*/
