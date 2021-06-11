using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;
public class AirChannelManager : MonoBehaviour
{
    public int ID;
    public List<GameObject> conditions = new List<GameObject>();
    void Start()
    {
        serviceLocator.interactableList.Add(ID, this.gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
