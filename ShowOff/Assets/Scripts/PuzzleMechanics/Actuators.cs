using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actuators : MonoBehaviour
{
    //AUTHOR: Ezra
    //SHORT DISCRIPTION: Handles basic logic for all actuators

    [HideInInspector] public int ID;
    public bool isActuated;
    public List<GameObject> interactables = new List<GameObject>();

}