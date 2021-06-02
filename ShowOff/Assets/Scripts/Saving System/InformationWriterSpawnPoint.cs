using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationWriterSpawnPoint : InformationWriter
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
