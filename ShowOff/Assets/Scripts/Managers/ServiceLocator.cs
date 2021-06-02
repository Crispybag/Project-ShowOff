using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ServiceLocator : MonoBehaviour
{
    //AUTHOR: Leo Jansen
    //SHORT DISCRIPTION: Allows for safe global access for items that want to subscribe to the service locator 
    //without having to add references, a FIND function, or a public static

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------
    public static ServiceLocator serviceLocator = null;


    //----------------------- private ------------------------
    private Dictionary<string, GameObject> _serviceList = new Dictionary<string, GameObject>();
    public Dictionary<int, GameObject> interactableList = new Dictionary<int, GameObject>();


    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (null == serviceLocator)
        {
            serviceLocator = this;
            return;
        }

        Destroy(gameObject);
    }

    void Update()
    {
        foreach (KeyValuePair<string, GameObject> obj in _serviceList)
        {
            if (null == obj.Value)
            {
                RemoveFromList(obj.Key);
                break;
            }
        }
    }
    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

   
    /// <summary>
    /// Adds a gameObject to the service list with a given key
    /// </summary>
    /// <param name="pName"> the key you want to give the object </param>
    /// <param name="pGameObject"> the gameObject you want to add to the list </param>
    public void AddToList(string pName, GameObject pGameObject)
    {
        //check if it is in list
        if (IsInList(pName))
        {
            //remove the old object from the list
            RemoveFromList(pName);
        }

        _serviceList.Add(pName, pGameObject);
    }

    /// <summary>
    ///gets the service object with given key <pName> from the list
    /// </summary>
    /// <param name="pName">key of the item in the service list </param>
    /// <returns>gameObject from serviceList with the key</returns>
    public GameObject GetFromList(string pName)
    {
        //check if list contains key
        if (_serviceList.ContainsKey(pName))
        {
            //check whether the object is not null
            if (_serviceList[pName] == null)
            {
                Debug.LogError("servicelist value " + pName + " was null. Has the object been destroyed?");
                return null;
            }
            return _serviceList[pName];
        }

        //couldn't find the key
        Debug.LogError("servicelist key: " + pName + " has not been found.");
        return null;
    }

    /// <summary>
    /// Gets the service list
    /// </summary>
    /// <returns> the service list </returns>
    public Dictionary<string, GameObject> GetList()
    {
        return _serviceList;
    }

    /// <summary>
    /// Removes the gameObject with the given key from the list
    /// </summary>
    /// <param name="pName">the key of the gameobject </param>
    public void RemoveFromList(string pName)
    {
        if (_serviceList.ContainsKey(pName))
        {
            _serviceList.Remove(pName);
        }
    }

    /// <summary>
    /// checks if the result from getting the key is null
    /// </summary>
    /// <param name="pName"> key to the gameObject</param>
    /// <returns> whether object is or is not in the list </returns>
    public bool IsObjNull(string pName)
    {
        if (!IsInList(pName)) return true;
        else if (GetFromList(pName) == null) return true;
        return false;
    }

    /// <summary>
    /// checks if the service list is in the list
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool IsInList(string name)
    {
        return (_serviceList.ContainsKey(name));
    }
}
