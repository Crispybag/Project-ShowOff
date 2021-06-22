using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;
using sharedAngy;
public class LevelInit : MonoBehaviour
{
    bool firstRun = true;


    public void Update()
    {
        if (firstRun)
        {
            ReqLevelName name = new ReqLevelName();
            name.levelName = serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().gameSceneName;
            serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().SendPackage(name);
            firstRun = false;
        }

    }
}
