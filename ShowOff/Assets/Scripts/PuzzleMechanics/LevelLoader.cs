using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;


/// <summary>
/// (Leo) Contains logic about the level loading mechanic (next scene)
/// </summary>

public class LevelLoader : MonoBehaviour
{

    public int ID;
    public List<GameObject> conditions = new List<GameObject>();

    void Start()
    {
        serviceLocator.interactableList.Add(ID, this.gameObject);
    }
}
