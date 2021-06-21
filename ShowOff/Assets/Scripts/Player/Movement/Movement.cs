using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

/// <summary>
/// (Leo) Contains logic about the all the movment in the scene
/// </summary>

public abstract class Movement : MonoBehaviour
{
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
        model.transform.rotation = Quaternion.Euler(new Vector3(model.transform.rotation.x, Mathf.LerpAngle(currentRotation.y, targetRotation.y, ratio), model.transform.rotation.z));

        //set to next position in line
        if (coords.Count != 0 && ratio > 0.99f)
        {
            moveToTile(coords[0]);
            SetRotation(coords[0]);
            coords.RemoveAt(0);
        }
        checkForMovement();

    }

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================
    public void disectMovementCommands(string pMovements)
    {
        List<string> movementCalls = pMovements.Split(' ').ToList();

        for (int i = 0; i < movementCalls.Count; i += 3)
        {
            try
            {
                float dirX = float.Parse(movementCalls[i], CultureInfo.InvariantCulture);
                float dirY = float.Parse(movementCalls[i + 1], CultureInfo.InvariantCulture);
                float dirZ = float.Parse(movementCalls[i + 2], CultureInfo.InvariantCulture);
                Vector3 vec = new Vector3(dirX, dirY, dirZ);
                coords.Add(vec);
            }
            catch
            {

            }
        }

    }
    public void SetRotation(Vector3 pDirection, int orientation = -1)
    {
        currentRotation = model.transform.rotation.eulerAngles;
        Vector3 rot = model.transform.rotation.eulerAngles;


        if (orientation == -1)
        {
            //set angle based on movement vector
            if (pDirection.x < 0)
            {
                orientation = 270;      //left
            }
            else if (pDirection.x > 0)
            {
                orientation = 90;       //right
            }
            else if (pDirection.z < 0)
            {
                orientation = 180;        //down
            }
            else if (pDirection.z > 0)
            {
                orientation = 0;      //up
            }
        }

        if (rot.y != orientation)
        {
            rot.y = orientation;
            targetRotation = rot;
        }
    }


    public void moveToTile(Vector3 pDirection, int orientation = -1)
    {
        _targetPosition = pDirection + transform.position;
        _currentPosition = transform.position;
        timer = 0f;

    }

    protected void checkForMovement()
    {
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
        }
    }
}