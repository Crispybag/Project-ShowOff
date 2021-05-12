using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirChannelTrigger : MonoBehaviour
{
    //AUTHOR: Ezra
    //SHORT DISCRIPTION: Put on any object to make sure they interact with Air channels, this will put them in the list to make sure they move.

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------


    //----------------------- private ------------------------




    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "AirChannel")
        {
            collision.GetComponent<AirChannel>().AddObject(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "AirChannel")
        {
            collision.GetComponent<AirChannel>().RemoveObject(this.gameObject);
        }
    }

}