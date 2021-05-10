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
    public List<GameObject> currentObjects = new List<GameObject>();

    //----------------------- private ------------------------

    [SerializeField] private GameObject activator;
    [SerializeField] private float _airSpeed = 5;

    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================
    private void Start()
    {
        isAirEnabled = activator.GetComponent<PuzzleManager>().isCompleted;
    }

    private void Update()
    {
        if (isAirEnabled)
        {
            MoveObjects();
        }
    }

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================

    private void MoveObjects()
    {
        foreach(GameObject obj in currentObjects)
        {
            obj.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * _airSpeed);
        }
    }

}