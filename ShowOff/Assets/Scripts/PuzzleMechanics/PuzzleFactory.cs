using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PuzzleFactory : MonoBehaviour
{
    //AUTHOR: Ezra
    //SHORT DISCRIPTION: Puzzle Factory

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------

    [HideInInspector]public int ID;
    public bool isActuated;
    public List<GameObject> interactables = new List<GameObject>();

    //----------------------- private ------------------------


    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================



    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    //will toggle all things that are linked with the actuator.
    public void ToggleMechanics()
    {
        if (gameObject.GetComponent<AirChannelToggle>() != null)
        {
            gameObject.GetComponent<AirChannelToggle>().ToggleAirChannel();
        }
    }

    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================

}