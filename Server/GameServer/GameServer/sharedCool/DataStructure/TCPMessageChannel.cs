using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace sharedCool
{
    public class TCPMessageChannel
    {
        //the client
        private TcpClient _client = null;                             
        
        //the stream
        private NetworkStream _stream = null;                      
        
        //client's IP
        private IPEndPoint _remoteEndPoint = null;

        //avoid double sending
        private ASerializable _lastMessage = null;
        private byte[] _lastBytes = null;

        //send message function

        public TCPMessageChannel(TcpClient pTcpClient)
        {
            _client = pTcpClient;
            _stream = _client.GetStream();
            _remoteEndPoint = _client.Client.RemoteEndPoint as IPEndPoint;
        }


        public void SendMessage(ASerializable pMessage)
        {
            try
            {
                //grab the required bytes from either the packet or the cache
                if (_lastMessage != pMessage)
                {
                    Packet outPacket = new Packet();
                    outPacket.Write(pMessage);
                    _lastBytes = outPacket.GetBytes();
                }

                StreamUtil.Write(_stream, _lastBytes);
            }
            catch (Exception e)
            {
            }
        }

        //Everything below is copy pasted and needs to be rewritten

        public bool Connected
        {
            get
            {
                return _client != null && _client.Connected;
            }
        }

        public bool HasMessage()
        {
            //we use an update StreamUtil.Available check instead of just Available > 0
            return Connected && StreamUtil.Available(_client);
        }


        public ASerializable ReceiveMessage()
        {

            try
            {

                byte[] inBytes = StreamUtil.Read(_stream);
                Packet inPacket = new Packet(inBytes);
                ASerializable inObject = inPacket.ReadObject();

                return inObject;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void Close()
        {
            try
            {
                _client.Close();
            }
            catch
            {
            }
            finally
            {
                _client = null;
            }
        }
    }
}
