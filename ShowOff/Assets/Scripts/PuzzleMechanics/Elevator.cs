using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;

public class Elevator : MonoBehaviour
{

    public int ID;

    private float timer;
    protected float _travelTime = 1f;

    protected Vector3 _currentPosition;
    private Vector3 _targetPosition;

    private void Start()
    {
        serviceLocator.interactableList.Add(ID, this.gameObject);
        _currentPosition = this.transform.position;
        _targetPosition = this.transform.position;
    }

    private void Update()
    {
        //lerp position
        timer += Time.deltaTime;
        float ratio = timer / _travelTime;
        transform.position = Vector3.Lerp(_currentPosition, _targetPosition, ratio);

        checkForMovement();
    }

    public void SetTargetPosition(int pX, int pY)
    {
        Debug.Log("Set the new position of the elevator to : " + pX + "," + pY + "!");
        _targetPosition.x = pX;
        _targetPosition.y = pY;
        _targetPosition.z = 0;

        _currentPosition = transform.position;
        timer = 0f;
    }
    protected void checkForMovement()
    {
        //makes sure we dont move multiple tiles within the same amount of time
        //because when holding the button id would stack if we wouldnt do this.
        if ((_targetPosition - transform.position).magnitude < 0.01f)
        {
            //canMove = true;
            transform.position = _targetPosition;
            _currentPosition = transform.position;
        }

    }

}
