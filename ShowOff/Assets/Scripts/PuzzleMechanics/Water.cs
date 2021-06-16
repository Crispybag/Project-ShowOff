using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;

public class Water : MonoBehaviour
{
    public int ID;
    public List<GameObject> waterLevelPoints = new List<GameObject>();


    private float timer;
    protected float _travelTime = 0.5f;

    protected Vector3 _currentPosition;
    private Vector3 _targetPosition;

    private void Start()
    {
        _currentPosition = waterLevelPoints[0].transform.position;
        _targetPosition = this.transform.position;
        serviceLocator.interactableList.Add(ID, this.gameObject);
    }

    private void Update()
    {
        //lerp position
        timer += Time.deltaTime;
        float ratio = timer / _travelTime;
        transform.position = Vector3.Lerp(_currentPosition, _targetPosition, ratio);

        checkForMovement();
    }

    public void SetTargetPosition(int pX, int pY, int pZ)
    {
        Debug.Log("Set the new position of the water level to : " + pX + "," + pY + "!");
        _targetPosition.x = pX;
        _targetPosition.y = pY;
        _targetPosition.z = pZ;

        _currentPosition = transform.position;
        timer = 0f;
    }
    protected void checkForMovement()
    {
        if ((_targetPosition - transform.position).magnitude < 0.01f)
        {
            transform.position = _targetPosition;
            _currentPosition = transform.position;
        }

    }
}
