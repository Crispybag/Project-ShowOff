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

        //receiveText();


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("trying to send a package");
            ReqKeyDown keyDown = new ReqKeyDown();
            keyDown.keyInput = ReqKeyDown.KeyType.UP;
            serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().SendPackage(keyDown);
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("trying to send a package");

            ReqKeyDown keyDown = new ReqKeyDown();
            keyDown.keyInput = ReqKeyDown.KeyType.DOWN;
            serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().SendPackage(keyDown);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("trying to send a package");

            ReqKeyDown keyDown = new ReqKeyDown();
            keyDown.keyInput = ReqKeyDown.KeyType.LEFT;
            serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().SendPackage(keyDown);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("trying to send a package");

            ReqKeyDown keyDown = new ReqKeyDown();
            keyDown.keyInput = ReqKeyDown.KeyType.RIGHT;
            serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().SendPackage(keyDown);
        }        
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("trying to send a package");

            ReqKeyDown keyDown = new ReqKeyDown();
            keyDown.keyInput = ReqKeyDown.KeyType.INTERACTION;
            serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().SendPackage(keyDown);
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
        Debug.Log("Handling :)");
        if (pInMessage is ConfMove)         { handleConfMove(pInMessage as ConfMove); }
        if (pInMessage is ConfJoinServer)   { handleConfJoin(pInMessage as ConfJoinServer); }
    }

    [SerializeField] private GameObject player0;
    [SerializeField] private GameObject player1;
    public void handleConfMove(ConfMove pMoveConfirm)
    {
        Debug.Log("We got a confirmed movement for the player : " + pMoveConfirm.player);
        Debug.Log("With the following data: x: " + pMoveConfirm.dirX + " y: " + pMoveConfirm.dirY );
        if (pMoveConfirm.player == 0)
        {
            player0.GetComponent<Movement>().moveToTile(new Vector3(pMoveConfirm.dirX, pMoveConfirm.dirY, pMoveConfirm.dirZ) - player0.transform.position);
            player0.transform.position = new Vector3(pMoveConfirm.dirX, pMoveConfirm.dirY, pMoveConfirm.dirZ);
            Debug.Log("Moved player 0!");
        }
        else
        {
            player1.GetComponent<Movement>().moveToTile(new Vector3(pMoveConfirm.dirX, pMoveConfirm.dirY, pMoveConfirm.dirZ) - player1.transform.position);
            //player1.transform.position = new Vector3(pMoveConfirm.dirX, pMoveConfirm.dirY, pMoveConfirm.dirZ);
            //player1.transform.position = new Vector3(-10, -10, pMoveConfirm.dirZ);
            Debug.Log("Moved player 1!");
        }
    }

    private Packet receivePacket()
    {
        try
        {
            byte[] inBytes = StreamUtil.Read(serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().client.GetStream());
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
        if (serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().client.Available > 0)
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
