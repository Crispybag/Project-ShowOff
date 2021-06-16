using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;
using sharedAngy;

public class DoorManager : MonoBehaviour
{
    //AUTHOR: Ezra 
    //SHORT DISCRIPTION: Doors handle conditions, when all of these return true, the door opens.

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------
    
    [FMODUnity.EventRef]
    public string eventPathClosed;

    
    [FMODUnity.EventRef]
    public string eventPathOpen;

    public int ID;

    //----------------------- private ------------------------

    public List<GameObject> conditions = new List<GameObject>();

    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================
    private void Start()
    {
        serviceLocator.interactableList.Add(ID, this.gameObject);
    }


    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================


    public void SetDoor(bool isOpen)
    {
        if (isOpen)
        {
            GetComponent<MeshRenderer>().enabled = false;
            serviceLocator.GetFromList("AudioManager").GetComponent<AudioManager>().playSound(eventPathClosed, this.gameObject);
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = true;
            serviceLocator.GetFromList("AudioManager").GetComponent<AudioManager>().playSound(eventPathOpen, this.gameObject);
        }
    }

    }