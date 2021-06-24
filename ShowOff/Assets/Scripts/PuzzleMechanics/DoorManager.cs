using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;
using sharedAngy;

/// <summary>
/// (Ezra) Contains logic about the door mechanic
/// </summary>

public class DoorManager : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string eventPathClosed;

    
    [FMODUnity.EventRef]
    public string eventPathOpen;

    public int ID;

    public List<GameObject> conditions = new List<GameObject>();


    private void Start()
    {
        serviceLocator.interactableList.Add(ID, this.gameObject);
    }

    public void SetDoor(bool isOpen)
    {
        transform.parent.GetComponentInChildren<Animator>().SetBool("isOpen", isOpen);
        if (isOpen)
        {
            
            serviceLocator.GetFromList("AudioManager").GetComponent<AudioManager>().playSound(eventPathClosed, this.gameObject);
        }
        else
        {
           
            serviceLocator.GetFromList("AudioManager").GetComponent<AudioManager>().playSound(eventPathOpen, this.gameObject);
        }
    }

    }