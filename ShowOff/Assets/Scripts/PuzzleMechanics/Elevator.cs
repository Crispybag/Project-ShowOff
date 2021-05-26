using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    public int ID;

    private float timer;
    protected float _travelTime = 0.1f;

    protected Vector3 _currentPosition;
    private Vector3 _targetPosition;

    private void Start()
    {
        _currentPosition = this.transform.position;
        _targetPosition = this.transform.position;
    }

    private void Update()
    {
        //lerp position
        timer += Time.deltaTime;
        float ratio = timer / _travelTime;
        transform.position = Vector3.Lerp(_currentPosition, _targetPosition, ratio);
    }

    public void SetTargetPosition(int pX, int pY)
    {
        _targetPosition.x = pX;
        _targetPosition.y = pY;
    }


}
