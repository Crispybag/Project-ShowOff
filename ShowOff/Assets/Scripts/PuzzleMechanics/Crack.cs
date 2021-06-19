using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;


/// <summary>
/// (Ezra) Contains logic about the crack mechanic
/// </summary>

public class Crack : Actuators
{

    public void Start()
    {
        serviceLocator.interactableList.Add(ID, this.gameObject);
    }

    public void FixCrack()
    {
        this.transform.parent.GetComponentInChildren<CrackAnimation>().fixing = true;
    }

}
