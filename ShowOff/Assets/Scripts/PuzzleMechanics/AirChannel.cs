using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirChannel : MonoBehaviour
{
    //AUTHOR: Ezra
    //SHORT DISCRIPTION:

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------

    public bool isAirEnabled = false;
    [HideInInspector] public List<GameObject> currentObjects = new List<GameObject>();

    //----------------------- private ------------------------

    [SerializeField] private float _airSpeed = 5;

    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================

    private void Update()
    {
        MoveObjects();
    }

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================

    private void MoveObjects()
    {
        if (isAirEnabled)
        {
            foreach (GameObject obj in currentObjects)
            {
                obj.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * _airSpeed);
            }
        }
    }

}