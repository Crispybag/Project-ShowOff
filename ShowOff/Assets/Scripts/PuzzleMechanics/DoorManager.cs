using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    //AUTHOR: Ezra 
    //SHORT DISCRIPTION: Doors handle conditions, when all of these return true, the door opens.

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------


    //----------------------- private ------------------------

    [SerializeField] private List<PuzzleManager> conditions = new List<PuzzleManager>();

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
        foreach (PuzzleManager obj in conditions)
        {
            i++;
            //if all conditions return true it destroys the door (later animation). If a single one is false, it breaks out.
            if (!obj.isCompleted)
            {
                break;
            }
            else if (i == conditions.Count)
            {
                i = 0;
                Destroy(this.gameObject);
            }
        }
    }

}