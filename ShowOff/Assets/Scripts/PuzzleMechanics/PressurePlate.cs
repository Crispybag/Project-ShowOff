using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;

/// <summary>
/// (Ezra) Contains logic about the pressure plate mechanic
/// </summary>

public class PressurePlate : Actuators
{
    [SerializeField] private GameObject pressureplateModel;
    [SerializeField] private Material activedMaterial;
    [SerializeField] private Material deactivedMaterial;

    private void Start()
    {
        serviceLocator.interactableList.Add(ID, this.gameObject);
    }

    public void UpdateActuator(bool isActive)
    {
        gameObject.GetComponent<PlaySingleSound>().PlaySoundOnce();
        transform.parent.GetComponentInChildren<Animator>().SetBool("isActive", isActive);
        if (isActive)
        {
            pressureplateModel.GetComponent<MeshRenderer>().material = activedMaterial;
        }
        else
        {
            pressureplateModel.GetComponent<MeshRenderer>().material = deactivedMaterial;
        }
    }
}
