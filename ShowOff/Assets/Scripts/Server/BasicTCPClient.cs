using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using sharedAngy;
using System.Text;
using static ServiceLocator;

public class BasicTCPClient : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string _hostname = "localhost";
    [SerializeField] private string _username = "Poggywoggy";
    [SerializeField] private int _port = 42069;
    public TcpClient _client;
    
    void Start()
    {
        //connectToServer();
    }

    // Update is called once per frame
    void Update()
    {

        receiveText();


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("trying to send a package");
            ReqKeyDown keyDown = new ReqKeyDown();
            keyDown.keyInput = ReqKeyDown.KeyType.UP;
            sendPackage(keyDown);
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("trying to send a package");

            ReqKeyDown keyDown = new ReqKeyDown();
            keyDown.keyInput = ReqKeyDown.KeyType.DOWN;
            sendPackage(keyDown);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("trying to send a package");

            ReqKeyDown keyDown = new ReqKeyDown();
            keyDown.keyInput = ReqKeyDown.KeyType.LEFT;
            sendPackage(keyDown);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("trying to send a package");

            ReqKeyDown keyDown = new ReqKeyDown();
            keyDown.keyInput = ReqKeyDown.KeyType.RIGHT;
            sendPackage(keyDown);
        }
    }

    private void connectToServer()
    {
        try
        {
            _client = new TcpClient();
            //connect to the ip
            _client.Connect(_hostname, _port);
            ReqJoinServer joinRequest = new ReqJoinServer();
            joinRequest.requestedName = _username;
            sendPackage(joinRequest);

        }
        catch
        {
            Debug.LogError("could not connect client to server");
        }
    }

    private void sendPackage(ASerializable pSerializable)
    {
        //create the packet
        Debug.Log("We are going to send a package");
        Packet _outPacket = new Packet();
        _outPacket.Write(pSerializable);
        //send package to the stream
        StreamUtil.Write(_client.GetStream(), _outPacket.GetBytes());
        Debug.Log("Sent the package to the stream");
    }


    private void handlePackage(ASerializable pInMessage)
    {
        if (pInMessage is ConfMove)         { handleConfMove(pInMessage as ConfMove); }
        if (pInMessage is ConfJoinServer)   { handleConfJoin(pInMessage as ConfJoinServer); }
    }

    private void handleConfMove(ConfMove pMoveConfirm)
    {
        GameObject player = serviceLocator.GetFromList(pMoveConfirm.name);
        player.GetComponent<Movement>().moveToTile(new Vector3(pMoveConfirm.dirX, pMoveConfirm.dirY, pMoveConfirm.dirZ));
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

    private void handleConfJoin(ConfJoinServer pJoinConfirm)
    {
        if (pJoinConfirm.acceptStatus)
        {
            Debug.Log("YAY :D, You connected");
        }
        else
        {
            Debug.Log("Not Accepted, same name probably");
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
