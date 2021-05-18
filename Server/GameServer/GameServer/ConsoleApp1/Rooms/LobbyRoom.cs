using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    class LobbyRoom : Room
    {
        public LobbyRoom(TCPGameServer pServer) : base(pServer) { }
        protected override void handleNetworkMessage(ASerializable pMessage, TCPMessageChannel pSender)
        {
            if(pMessage is ChatMessage) { handleChatMessage(pMessage as ChatMessage, pSender); }
        }

        private void handleChatMessage(ChatMessage pMessage, TCPMessageChannel pSender)
        {
            ChatMessage newMessage = new ChatMessage();
            newMessage.textMessage = "[" + _server.allConnectedUsers[pSender].GetPlayerName() + "] : " + pMessage.textMessage;
            sendToAll(newMessage);
        }

        public void AddMember(TCPMessageChannel pChannel)
        {
            Console.WriteLine("User joined lobby room");
            addMember(pChannel);
        }

    }
}
