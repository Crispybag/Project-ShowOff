using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using sharedAngy;

public class ChatHandler : MonoBehaviour
{

    public Text chatText;
    public Text inputChat;


    public void SendMessageToServer()
    {
        if (null != inputChat.text)
        {
            ChatMessage newChat = new ChatMessage();
            newChat.textMessage = inputChat.text;
            FindObjectOfType<ClientManager>().sendPackage(newChat);
            inputChat.text = "";
        }
    }

    public void AppendChatMessage(string pMessage)
    {
        chatText.text += pMessage + "\n";
    }

}
