using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (Leo) Writes necessary information about the slope mechanic
/// </summary>

public class IWSlope : InformationWriter
{
    // Start is called before the first frame update
    public override void StartSave()
    {
        base.StartSave();
    }

    public override void WriteAllInformation()
    {
        base.WriteAllInformation();
        float rot = transform.rotation.eulerAngles.y;
        if(rot >= -45f && rot <= 45)
        {
            rot = 0;
        }
        else if (rot >= 45f && rot <= 135)
        {
            rot = 90;
        }
        else if (rot >= -135f && rot <= -45 || rot > 225 && rot <= 315)
        {
            rot = -90;
        }
        else
        {
            rot = 180;
        }
        AddToInformation(rot);
    }


}
