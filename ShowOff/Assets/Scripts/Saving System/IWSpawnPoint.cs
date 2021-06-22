using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (Leo) Writes necessary information about the spawn points
/// </summary>
public class IWSpawnPoint : InformationWriter
{

    [SerializeField] private int playerIndex = 0;
    // Start is called before the first frame updatr
    public override void StartSave()
    {
        base.StartSave();
    }

    // Update is called once per frame
    public override void WriteAllInformation()
    {
        base.WriteAllInformation();
        AddToInformation(playerIndex);
    }

}
