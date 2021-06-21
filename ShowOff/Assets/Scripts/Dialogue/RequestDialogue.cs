using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;
using sharedAngy;

/// <summary>
/// (Ezra) Send package to server to handle dialogue.
/// </summary>

public class RequestDialogue : MonoBehaviour
{
    public void progressDialogueRequest()
    {
        ReqProgressDialogue progress = new ReqProgressDialogue();
        serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().SendPackage(progress);
    }
}
