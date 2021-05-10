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

    //----------------------- private ------------------------

    [SerializeField] private GameObject activator;
    public List<GameObject> currentObjects = new List<GameObject>();

    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================
    private void Start()
    {
        isAirEnabled = activator.GetComponent<PuzzleManager>().isCompleted;
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

}