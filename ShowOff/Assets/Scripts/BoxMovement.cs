using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sharedAngy;
using static ServiceLocator;

public class BoxMovement : MonoBehaviour
{

    [HideInInspector] public int ID;
    private float timer;
    [SerializeField] private float _travelTime;
    [SerializeField] private Vector3 _currentPosition;
    [SerializeField] private Vector3 _targetPosition;

    private void Start()
    {
        serviceLocator.interactableList.Add(ID, this.gameObject);
        _currentPosition = transform.position;
        _targetPosition = transform.position;
    }

    public void UpdateBox(bool isPickedUp, int posX, int posY, int posZ)
    {
        _currentPosition = transform.position;
        _targetPosition = new Vector3(posX, posY, posZ);

        timer = 0f;
    }

    protected virtual void Update()
    {
        //lerp position
        timer += Time.deltaTime;
        float ratio = timer / _travelTime;
        transform.position = Vector3.Lerp(_currentPosition, _targetPosition, ratio);
    }
}
