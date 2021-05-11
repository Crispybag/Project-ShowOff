using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightMovement : Movement
{
    //AUTHOR: Ezra 
    //SHORT DISCRIPTION:

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------


    //----------------------- private ------------------------


    private Vector3 _direction;
    private bool wallCheckCalled;

    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    override public bool wallCheck(Vector3 pTargetPosition, Vector3 pCurrentPosition, int calls = 0)
    {
        bool isWall = base.wallCheck(pTargetPosition, pCurrentPosition, calls);
        if (calls > ServiceLocator.serviceLocator.GetFromList("Player1").GetComponent<PlayerMovement>().playerPushWeight)//weight)
        {
            return true;
        }
        if (!isWall)
        {
            wallCheckCalled = true;
            _direction = pTargetPosition - pCurrentPosition;
        }
        return isWall;
    }

    protected override void Update()
    {
        if (wallCheckCalled)
        {
            moveToTile(_direction);
        }
        base.Update();
        wallCheckCalled = false;
    }


    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================

}