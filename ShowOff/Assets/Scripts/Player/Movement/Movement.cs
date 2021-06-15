using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
/*
    public Vector3 toBePosition;     //position it will be in after lerp

    public Vector3 leftDirection;
    public Vector3 rightDirection;*/

    //----------------------- private ------------------------

    private int terrainLayer = 6;
    private int movableLayer = 7;

    protected Vector3 _currentPosition;
    private Vector3 _targetPosition;
    
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    protected float _travelTime = 0.05f;
    private float timer;

    public bool canMove = true;
    GameObject model;
    private AnimationHandler animation;

    private float animatorCooldown;

    private List<Vector3> coords = new List<Vector3>();


    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================
    protected virtual void Start()
    {
        //toBePosition = transform.position;
        _currentPosition = transform.position;
        _targetPosition = transform.position;
        model = transform.Find("Model").gameObject;
        animation = this.transform.GetComponent<AnimationHandler>();
    }

    protected virtual void Update()
    {
        //lerp position
        timer += Time.deltaTime;
        float ratio = timer / _travelTime;
        transform.position = Vector3.Lerp(_currentPosition, _targetPosition, ratio);
        model.transform.rotation = Quaternion.Euler(Vector3.Lerp(currentRotation, targetRotation, ratio));

        //set to next position in line
        if (coords.Count != 0 && ratio > 0.99f)
        {
            moveToTile(coords[0]);
            coords.RemoveAt(0);
        }


        checkForMovement();

    }


/*    public void checkForFalling()
    {
        if (canMove)
        {
            if (Physics.Raycast(_currentPosition, -Vector3.up, out RaycastHit hit, 1f))
            {
                //if we hit something that isnt a airchannel nor a terrain, it will move down.
                if (hit.collider.gameObject.tag != "AirChannel" && hit.collider.gameObject.layer != terrainLayer)
                {
                    moveToTile(-Vector3.up);
                }
            }
            //didnt detect anything, thus we need to fall
            else
            {
                moveToTile(-Vector3.up);
            }
        }
    }*/

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================
    public void disectMovementCommands(string pMovements)
    {
        Debug.Log(pMovements);
        //coords.Clear();
        List<string> movementCalls = pMovements.Split(' ').ToList();

        for (int i = 0; i < movementCalls.Count; i += 3)
        {
            try
            {
                float dirX = float.Parse(movementCalls[i]);
                float dirY = float.Parse(movementCalls[i + 1]);
                float dirZ = float.Parse(movementCalls[i + 2]);
                Vector3 vec = new Vector3(dirX, dirY, dirZ);
                coords.Add(vec);
            }
            catch
            {

            }
        }

        Debug.Log(coords.Count);

    }


    public void moveToTile(Vector3 pDirection, int orientation = -1)
    {

        //get normalized direction just makes sure the direction on the xyz is always either 0 or 1. (sometimes it would be 0.0000001)
        //pDirection = getNormalizedDirection(pDirection);
        //if there isnt a wall update our target position to where we want to go.
        //_targetPosition = pDirection + _currentPosition;
        _targetPosition = pDirection + transform.position;
        _currentPosition = transform.position;

        currentRotation = model.transform.rotation.eulerAngles;
        Vector3 rot = model.transform.rotation.eulerAngles;
        if(orientation == 180)
        {
            orientation = 0;
        }
        else if(orientation == 0)
        {
            orientation = 180;
        }
        else if (orientation == -90)
        {
                orientation = 270;
        }
        if (rot.y != orientation)
        {
            rot.y = orientation;
            targetRotation = rot;
        }
        
/*        float angle = ((model.transform.rotation.eulerAngles.y - orientation + 540) % 360) - 180;
        if(angle > 0)
        {
            rot.y = orientation;
        }
        else
        {
            rot.y = orientation - 360;
        }*/


        //targetRotation = rot;

        //toBePosition = _targetPosition;
        timer = 0f;
            
    }

    protected void checkForMovement()
    {
        //makes sure we dont move multiple tiles within the same amount of time
        //because when holding the button id would stack if we wouldnt do this.
        canMove = true;

        if ((_targetPosition - transform.position).magnitude < 0.01f)
        {
            //canMove = true;
            transform.position = _targetPosition;
            _currentPosition = transform.position;
            if (animatorCooldown >= 0.1f)
            {
                animation.isWalking = false;
            }
            animatorCooldown += Time.deltaTime;
            
        }
        else
        {
            animatorCooldown = 0;
            animation.isWalking = true;

            //canMove = false;
        }
    }
    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================


/*    virtual public bool wallCheck(Vector3 pTargetPosition, Vector3 pCurrentPosition)
    {
        //get the direction and make sure they are either 0 or 1 again.
        Vector3 moveDirection = (pTargetPosition - pCurrentPosition).normalized;
        moveDirection = getNormalizedDirection(moveDirection);

        //calculate the left and right tile of the forward tile.
         leftDirection = getLeftFromDirection(moveDirection);
         rightDirection = getRightFromDirection(moveDirection);

        //debug rays to visualize the raycasts.
        Debug.DrawRay(pCurrentPosition - moveDirection * 0.1f, moveDirection * 1.4f, Color.green , 5);
        Debug.DrawRay(pCurrentPosition - moveDirection * 0.1f, leftDirection * 1.4f, Color.green , 5);
        Debug.DrawRay(pCurrentPosition - moveDirection * 0.1f, rightDirection * 1.4f, Color.green, 5);

        
        //============================== Collision Checks ===================================


        //first we check right in front of us.
        if (Physics.Raycast(pCurrentPosition, moveDirection, out RaycastHit frontHit, 1.4f))
        {
            //if we hit terrain we return true because it means we hit a wall
            if (frontHit.collider.gameObject.layer == terrainLayer)
            {
                return true;
            }
            //if we hit a something on a movable layer, we will start a recursive loop to check if there is a empty spot to move into.
            if (frontHit.collider.gameObject.layer == movableLayer)
            {
                return frontHit.collider.gameObject.GetComponent<Movement>().wallCheck(pTargetPosition + moveDirection, pTargetPosition);
            }
            else { return false; }
        }


        //now we check on the left side (for the corner collision)
        if (Physics.Raycast(pCurrentPosition, leftDirection, out RaycastHit leftHit, 1.45f))
        {
            if (leftHit.collider.gameObject.layer == movableLayer)
            {
                //if we hit a movable layer and its also moving into the same position as we are, then move us back 1 tile.
                //even though i return true, and the code should stop and not move anymore, this would still happen, thus needed to move 1 back
                //a fix for this would be appreciated.
                if (pTargetPosition == leftHit.collider.gameObject.GetComponent<Movement>().toBePosition)
                {
                    moveToTile(moveDirection *= -1f);
                    return true;
                }
                else { return false; }
            }
            else { return false; }
        }



        //We do the same for the right side as we did with the left side.
        if (Physics.Raycast(pCurrentPosition, rightDirection, out RaycastHit rightHit, 1.45f))
        {
            if (rightHit.collider.gameObject.layer == movableLayer)
            {
                if (pTargetPosition == rightHit.collider.gameObject.GetComponent<Movement>().toBePosition)
                {
                    moveToTile(moveDirection *= -1f);
                    return true;
                }
                else { return false; }
            }
            else { return false; }
        }

        else { return false; }
    }

    protected Vector3 getNormalizedDirection(Vector3 oldDirection)
    {
        //makes sure everything is either 0 or 1.
        Vector3 newDirection = oldDirection;
        if (newDirection.x > 0.1f)
        {
            newDirection.x = 1;
        }
        else if (newDirection.x < -0.1f)
        {
            newDirection.x = -1;
        }
        else
        {
            newDirection.x = 0;
        }

        if (newDirection.z > 0.1f)
        {
            newDirection.z = 1;
        }
        else if (newDirection.z < -0.1f)
        {
            newDirection.z = -1;
        }
        else
        {
            newDirection.z = 0;
        }
        return newDirection;
    }

    private Vector3 getLeftFromDirection(Vector3 pDirection)
    {
        //calulcates the tile on the left side from given direction
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
        //calculates the tile on the right side from the given direction.
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
    }*/

    /*
    public bool WallFarCheck(Vector3 pTargetPosition, Vector3 pCurrentPosition)
    {
        Vector3 moveDirection = pTargetPosition - pCurrentPosition;
        moveDirection = getNormalizedDirection(moveDirection);
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

    */
    
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
