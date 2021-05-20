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


    //----------------------- private ------------------------

    [SerializeField] private List<PuzzleFactory> conditions = new List<PuzzleFactory>();

    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================
    private void Start()
    {

    }

    private void Update()
    {
        //checks whether all the conditions return true
        CheckConditions();
    }

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================


    private void CheckConditions()
    {
        int i = 0;
        foreach (PuzzleFactory obj in conditions)
        {
            i++;
            //if all conditions return true it destroys the door (later animation). If a single one is false, it breaks out.
            if (!obj.isActuated)
            {
                break;
            }
            else if (i == conditions.Count)
            {
                i = 0;
                ConfDoorToggle doorToggle = new ConfDoorToggle();
                doorToggle.posX = (int)this.gameObject.transform.position.x;
                doorToggle.posY = (int)this.gameObject.transform.position.y;
                serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().SendPackage(doorToggle);
                Destroy(this.gameObject);
            }
        }
    }

}