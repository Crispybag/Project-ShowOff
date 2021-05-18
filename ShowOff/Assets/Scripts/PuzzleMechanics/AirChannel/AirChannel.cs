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
    private List<GameObject> currentObjects = new List<GameObject>();

    //----------------------- private ------------------------


    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================

    private void Start()
    {
    }

    private void Update()
    {
        MoveObjects();
    }

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    public void AddObject(GameObject pObject)
    {
        currentObjects.Add(pObject);
        if (pObject.GetComponent<Movement>())
        {
            //sets state to airing which means its being influences by airchannels
            //pObject.GetComponent<Movement>().currentState = Movement.State.AIRING;
        }
    }

    public void RemoveObject(GameObject pObject)
    {
        currentObjects.Remove(pObject);
        if (pObject.GetComponent<Movement>())
        {
            //makes sure the state switches to something else besides airing
                //pObject.GetComponent<Movement>().currentState = Movement.State.GROUNDED;
        }
    }

    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================

    private void MoveObjects()
    {
        if (isAirEnabled)
        {
            foreach (GameObject obj in currentObjects)
            {
                if(obj.GetComponent<Movement>() != null)
                {
                        obj.GetComponent<Movement>().moveToTile(this.gameObject.transform.forward);
                }
            }
        }
    }



}






//obj.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward);
//obj.transform.position += this.gameObject.transform.forward;
