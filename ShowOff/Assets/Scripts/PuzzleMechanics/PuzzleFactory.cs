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

    public bool isActuated;

    //----------------------- private ------------------------


    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================



    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    abstract public void FinishMechanic();

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