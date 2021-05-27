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

    public int ID;

    //----------------------- private ------------------------

    [SerializeField] private List<int> conditionsID = new List<int>();

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
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
    }

    }