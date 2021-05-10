using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirChannelTrigger : MonoBehaviour
{
    //AUTHOR:
    //SHORT DISCRIPTION:

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------


    //----------------------- private ------------------------


    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================
    private void Start()
    {

    }

    private void Update()
    {

    }

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Box enters : " + collision.transform.tag +  " !");
        if(collision.transform.tag == "AirChannel")
        {
            Debug.Log("Box has entered air channel!");
            collision.transform.GetComponent<AirChannel>().currentObjects.Add(this.gameObject);
        }
    }

/*    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "AirChannel")
        {
            collision.transform.GetComponent<AirChannel>().currentObjects.Remove(this.gameObject);
        }
    }*/

}