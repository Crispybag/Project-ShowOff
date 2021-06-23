using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;
using sharedAngy;
public class LevelInit : MonoBehaviour
{
    bool firstRun = true;
    int calls = 0;

    public void Update()
    {
        calls++;
        if (firstRun && calls > 10)
        {
            Debug.Log("sent level request package");
            ReqLevelName name = new ReqLevelName();
            name.levelName = serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().gameSceneName;
            serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().SendPackage(name);
            firstRun = false;
        }

    }
}
