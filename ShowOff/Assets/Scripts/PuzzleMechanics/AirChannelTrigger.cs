using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirChannelTrigger : MonoBehaviour
{
    //AUTHOR: Ezra
    //SHORT DISCRIPTION:

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------


    //----------------------- private ------------------------

    [SerializeField] private float _airSpeed;


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

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "AirChannel")
        {
            collision.GetComponent<AirChannel>().currentObjects.Add(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "AirChannel")
        {
            collision.GetComponent<AirChannel>().currentObjects.Remove(this.gameObject);
        }
    }

}