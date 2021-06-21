using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using sharedAngy;
using static ServiceLocator;


/// <summary>
///  (Ezra) Handles incomming messages.
/// </summary>
public class ChatHandler : MonoBehaviour
{

    public Text chatText;
    public InputField inputChat;

    public void Start()
    {
        ReqLevelName name = new ReqLevelName();
        name.levelName = serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().gameSceneName;
        serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().SendPackage(name);
    }


    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            SendMessageToServer();
        }
    }

    public void SendMessageToServer()
    {
        if (null != inputChat.text && inputChat.text.Length != 0)
        {
            ChatMessage newChat = new ChatMessage();
            newChat.textMessage = inputChat.text;
            FindObjectOfType<ClientManager>().SendPackage(newChat);
            inputChat.text = "";
        }
    }

    public void AppendChatMessage(string pMessage)
    {
        chatText.text += pMessage + "\n";
    }

}
