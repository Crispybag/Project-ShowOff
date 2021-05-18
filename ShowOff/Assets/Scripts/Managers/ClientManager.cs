using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using sharedAngy;
using static ServiceLocator;
using UnityEngine.UI;

public class ClientManager : MonoBehaviour
{
    //AUTHOR:
    //SHORT DISCRIPTION:

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------

    public TcpClient client;

    //----------------------- private ------------------------

    [SerializeField] private string _hostname = "localhost";
    [SerializeField] private string _username = "Guest";
    [SerializeField] private int _port = 42069;
    [SerializeField] private InputField _ipAdressInput;
    [SerializeField] private InputField _userNameInput;
    [SerializeField] private Text _feedbackText;

    private ClientManager _clientManager;

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

        Destroy(gameObject);
    }

    void Start()
    {
        _feedbackText.text = "";
        _ipAdressInput.text = "localhost";
        _userNameInput.text = "Guest1";
        //connectToServer();
    }

    // Update is called once per frame
    void Update()
    {
        receiveInCommingData();
    }

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================

    #region ConnectToServer
    public void ConnectToServer()
    {
        _hostname = _ipAdressInput.text;
        _username = _userNameInput.text;

        try
        {
            client = new TcpClient();
            //connect to the ip
            client.Connect(_hostname, _port);
            ReqJoinServer joinRequest = new ReqJoinServer();
            joinRequest.requestedName = _username;
            sendPackage(joinRequest);
        }
        catch
        {
            Debug.LogError("could not connect client to server");
        }
    }

    private void handleConfJoin(ConfJoinServer pJoinConfirm)
    {
        if (pJoinConfirm.acceptStatus)
        {
            Debug.Log("YAY :D, You connected");
            serviceLocator.GetFromList("SceneManager").GetComponent<SceneManagerScript>().LoadSceneSingle("Lobby");
        }
        else
        {
            _feedbackText.text = pJoinConfirm.message;
            Debug.Log("Not Accepted, same name probably");
        }
    }

    #endregion


    private void handlePackage(ASerializable pInMessage)
    {
        if (pInMessage is ConfJoinServer) { handleConfJoin(pInMessage as ConfJoinServer); }
        if (pInMessage is ChatMessage) { handleChatMessage(pInMessage as ChatMessage); }
    }

    private void handleChatMessage(ChatMessage pMessage)
    {
        FindObjectOfType<ChatHandler>().AppendChatMessage(pMessage.textMessage);
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

    public void sendPackage(ASerializable pSerializable)
    {
        //create the packet
        Packet _outPacket = new Packet();
        _outPacket.Write(pSerializable);
        //send package to the stream
        StreamUtil.Write(client.GetStream(), _outPacket.GetBytes());
    }

}
