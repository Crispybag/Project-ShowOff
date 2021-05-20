using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using sharedAngy;
using static ServiceLocator;
using UnityEngine.UI;

public class ClientManager : MonoBehaviour
{
    //AUTHOR: Ezra
    //SHORT DISCRIPTION: Handles all incomming data from the server and makes sure it gets handled correctly.

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------

    public TcpClient client;

    //----------------------- private ------------------------

    private static ClientManager _clientManager;

    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (null == _clientManager)
        {
            _clientManager = this;
            return;
        }
        Destroy(this.gameObject);
    }

    void Start()
    {

        serviceLocator.AddToList("ClientManager", this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        receiveInCommingData();
    }

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    public void SendPackage(ASerializable pSerializable)
    {
        //create the packet
        Packet _outPacket = new Packet();
        _outPacket.Write(pSerializable);
        //send package to the stream
        StreamUtil.Write(client.GetStream(), _outPacket.GetBytes());
    }

    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================

    private void handlePackage(ASerializable pInMessage)
    {
        if (pInMessage is ConfJoinServer) { handleConfJoin(pInMessage as ConfJoinServer); }
        if (pInMessage is ChatMessage) { handleChatMessage(pInMessage as ChatMessage); }
        if (pInMessage is ConfJoinRoom) { handleConfJoinRoom(pInMessage as ConfJoinRoom); }
        if (pInMessage is ConfMove) { handleConfMove(pInMessage as ConfMove); }
        if (pInMessage is ConfActuatorToggle) { handleConfActuatorToggle(pInMessage as ConfActuatorToggle); }
    }


    private void handleConfActuatorToggle(ConfActuatorToggle pMessage)
    {
        Lever[] levers = FindObjectsOfType<Lever>();
        foreach(Lever lever in levers)
        {
            if(lever.gameObject.transform.position.x == pMessage.posX && lever.gameObject.transform.position.y == pMessage.posY)
            {
                lever.SetActivatedLever();
            }
        }
    }
    private void handleConfMove(ConfMove pMessage)
    {
        FindObjectOfType<BasicTCPClient>().handleConfMove(pMessage);
    }

    private void handleConfJoinRoom(ConfJoinRoom pMessage)
    {
        switch ((int)pMessage.room)
        {
            case 0: //login
                serviceLocator.GetFromList("SceneManager").GetComponent<SceneManagerScript>().LoadSceneSingle("MainMenu");
                break;
            case 1: //lobby
                serviceLocator.GetFromList("SceneManager").GetComponent<SceneManagerScript>().LoadSceneSingle("Lobby");
                break;
            case 2: //game
                serviceLocator.GetFromList("SceneManager").GetComponent<SceneManagerScript>().LoadSceneSingle("ClientTest");
                break;
            default:
                Debug.LogError("Given number is not able to be handled in client manager.");
                break;
        }

    }

    private void handleChatMessage(ChatMessage pMessage)
    {
        try
        {
            FindObjectOfType<ChatHandler>().AppendChatMessage(pMessage.textMessage);
        }
        catch
        {
            Debug.LogError("Could not find a chat handler in scene.");
        }
    }

    private void handleConfJoin(ConfJoinServer pJoinConfirm)
    {
        try
        {
            FindObjectOfType<LoginHandler>().handleConfJoin(pJoinConfirm);
        }
        catch
        {
            Debug.LogError("Could not find a login handler in scene.");
        }
    }



    #region ReadIncommingData

    private void receiveInCommingData()
    {
        if (null != client)
        {
            if (client.Available > 0)
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

    private Packet receivePacket()
    {
        try
        {
            byte[] inBytes = StreamUtil.Read(client.GetStream());
            Packet packet = null;
            if (inBytes.Length != 0)
            {
                packet = new Packet(inBytes);
            }
            return packet;
        }
        catch
        {
            client.Close();
            return null;
        }
    }

    #endregion



}
