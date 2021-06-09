using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;
public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update
    
    public int ID;
    public List<GameObject> conditions = new List<GameObject>();

    void Start()
    {
        serviceLocator.interactableList.Add(ID, this.gameObject);
    }
}
