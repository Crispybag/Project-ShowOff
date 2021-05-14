using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using Shared;
using System.Text;

public class BasicTCPClient : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string _hostname = "localhost";
    [SerializeField] private int _port = 42069;
    private TcpClient _client;

    void Start()
    {
        connectToServer();
    }

    // Update is called once per frame
    void Update()
    {

        //receiveText();


        if(Input.GetKeyDown(KeyCode.O))
        {
            sendPackage();
        }
        
    }

    private void connectToServer()
    {
        try
        {
            _client = new TcpClient();
            //connect to the ip
            _client.Connect(_hostname, _port);
        }
        catch
        {
            Debug.LogError("could not connect client to server");
        }
    }

    private void sendPackage()
    {
        //create the packet
        ReqAddCount reqAddCount = new ReqAddCount();
        Packet _outPacket = new Packet();
        _outPacket.Write(reqAddCount);
        //send package to the stream
        StreamUtil.Write(_client.GetStream(), _outPacket.GetBytes());
    }

    private void handleConfAddCount(ConfAddCount pAddCount)
    {
        int count = pAddCount.totalCount;
    }

    private void handlePackage(ASerializable pInMessage)
    {
        if (pInMessage is ConfAddCount) { handleConfAddCount(pInMessage as ConfAddCount); }
    }

    private Packet receivePacket()
    {
        try
        {
            byte[] inBytes = StreamUtil.Read(_client.GetStream());
            Packet packet = null;
            if (inBytes.Length != 0)
            {
                packet = new Packet(inBytes);
            }
            return packet;
        }
        catch
        {
            _client.Close();
            return null;
        }
    }
    private void receiveText()
    {
        if (_client.Available > 0)
        {
            //receive the packet
            Packet packet = receivePacket();

            if (null == packet)
            {
                Debug.Log("received Packet is null");
                return;
            }

            while (!packet.isReaderEmpty())
            {
                //unwrap the packet
                ASerializable inObject = packet.ReadObject();
                handlePackage(inObject);

            }
        }
    }


}
