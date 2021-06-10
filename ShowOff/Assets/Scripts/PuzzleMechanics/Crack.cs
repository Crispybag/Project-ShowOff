using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;

public class Crack : PuzzleFactory
{

    public void Start()
    {
        serviceLocator.interactableList.Add(ID, this.gameObject);
    }

    public void FixCrack()
    {
        Destroy(this.gameObject);
    }

}
