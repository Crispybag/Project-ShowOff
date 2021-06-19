using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;

/// <summary>
/// (Ezra) Contains logic about the pressure plate mechanic
/// </summary>

public class PressurePlate : Actuators
{
    private List<GameObject> _currentObjects = new List<GameObject>();
    [SerializeField] private Material mat1;
    [SerializeField] private Material mat2;

    private void Start()
    {
        serviceLocator.interactableList.Add(ID, this.gameObject);
    }

    public void UpdateActuator(bool isActive)
    {
        gameObject.GetComponent<PlaySingleSound>().PlaySoundOnce();
    }
}
