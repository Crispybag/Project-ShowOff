using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using sharedAngy;
using static ServiceLocator;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClientManager : MonoBehaviour
{
    //AUTHOR: Ezra
    //SHORT DISCRIPTION: Handles all incomming data from the server and makes sure it gets handled correctly.

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------

    public TcpClient client;
    public string ClientName;
    [HideInInspector] public int playersWantToReset = 0;
    private bool isHoldingBox = false;

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
        if (pInMessage is ConfDoorToggle) { handleConfDoorToggle(pInMessage as ConfDoorToggle); }
        if (pInMessage is ConfElevatorMove) { handleConfElevatorMove(pInMessage as ConfElevatorMove); }
        if (pInMessage is BoxInfo) { handleBoxInfo(pInMessage as BoxInfo); }
        if (pInMessage is ConfPlayer) { handlePlayerInfo(pInMessage as ConfPlayer); }
        if (pInMessage is ConfProgressDialogue) { handleProgressDialogue(pInMessage as ConfProgressDialogue); }
        if (pInMessage is ConfReloadScene) {  handleReloadScene(pInMessage as ConfReloadScene);  }
        if (pInMessage is ConfLoadScene) { handleLoadScene(pInMessage as ConfLoadScene); }
        if (pInMessage is ConfWaterPool) { handleConfWaterPool(pInMessage as ConfWaterPool); }
        if (pInMessage is ConfPlayerSwitch) { handleConfPlayerSwitch(pInMessage as ConfPlayerSwitch); }
        if (pInMessage is ConfAnimation) { handleConfAnimation(pInMessage as ConfAnimation); }
    }

    private void handleConfAnimation(ConfAnimation pMessage)
    {
        GameObject player1 = serviceLocator.GetFromList("Player1");
        GameObject player2 = serviceLocator.GetFromList("Player2");


        if (pMessage.player == 0)
        {
            if (pMessage.isFalling && !player1.GetComponent<AnimationHandler>().isFalling)
            {
                player1.GetComponent<AnimationHandler>().DoTrigger("startFalling");
                player1.GetComponent<AnimationHandler>().isFalling = true;
            }
            if (pMessage.isCrawling)
            {
                player1.GetComponent<AnimationHandler>().DoTrigger("startCrawling");
            }
            else
            {
                player1.GetComponent<AnimationHandler>().DoTrigger("stopCrawling");
            }
        }
        else
        {
            if (pMessage.isFalling && !player2.GetComponent<AnimationHandler>().isFalling)
            {
                player2.GetComponent<AnimationHandler>().DoTrigger("startFalling");
                player2.GetComponent<AnimationHandler>().isFalling = true;
            }
            if (pMessage.isCrawling)
            {
                player2.GetComponent<AnimationHandler>().DoTrigger("startCrawling");
            }
            else
            {
                player2.GetComponent<AnimationHandler>().DoTrigger("stopCrawling");
            }
        }
    }


    private void handleConfPlayerSwitch(ConfPlayerSwitch pMessage)
    {
        FindObjectOfType<HandleCharacterSelection>().UpdateCharacters(pMessage.playerIndex);
    }

    private void handleConfWaterPool(ConfWaterPool pMessage)
    {
        GameObject water = serviceLocator.interactableList[pMessage.ID];
        water.GetComponent<Water>().SetTargetPosition(pMessage.x, pMessage.y, pMessage.z);
    }

    private void handleLoadScene(ConfLoadScene pMessage)
    {
        SceneManagerScript sceneManager = serviceLocator.GetFromList("SceneManager").GetComponent<SceneManagerScript>();
        playersWantToReset = pMessage.playersReset;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == "Options")
            {
                serviceLocator.GetFromList("Options").GetComponent<Options>().UpdateText();
            }
        }

        if (pMessage.isResetting)
        {
            serviceLocator.ClearInteractables();
            sceneManager.LoadSceneSingle(pMessage.sceneName);
        }
    }


    private void handleReloadScene(ConfReloadScene pMessage)
    {
        SceneManagerScript sceneManager = serviceLocator.GetFromList("SceneManager").GetComponent<SceneManagerScript>();
        playersWantToReset = pMessage.playersReset;
        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            if(SceneManager.GetSceneAt(i).name == "Options")
            {
                serviceLocator.GetFromList("Options").GetComponent<Options>().UpdateText();
            }
        }

        if (pMessage.isResetting)
        {
            serviceLocator.ClearInteractables();
            sceneManager.LoadSceneSingle(pMessage.sceneName);
        }
    }

    private void handleProgressDialogue(ConfProgressDialogue pMessage)
    {
        serviceLocator.interactableList[pMessage.ID].GetComponent<Dialogue>().ProgressDialogue();
    }

    private void handlePlayerInfo(ConfPlayer pMessage)
    {
        ClientName = pMessage.playerName;
        Debug.Log("Joined game as " + ClientName);
        try
        {
            serviceLocator.GetFromList("CameraManager").GetComponent<CameraManager>()._playerCameraGameObject = serviceLocator.GetFromList(ClientName).transform.Find("CameraPosition").gameObject;
            InteractableShaderManager[] shaders = FindObjectsOfType<InteractableShaderManager>();
            foreach(InteractableShaderManager shader in shaders)
            {
                shader.SetupShader(pMessage.playerName);
            }
        }
        catch
        {
            Debug.LogError("Could not find gameobjects for camera movement!");
        }
    }

    private void handleConfElevatorMove(ConfElevatorMove pMessage)
    {
        Debug.Log("Got a elevator move with ID: " + pMessage.ID + "!");
        GameObject obj = serviceLocator.interactableList[pMessage.ID];
        obj.GetComponent<Elevator>().SetTargetPosition(pMessage.posX, pMessage.posY, pMessage.posZ);
    }

    private void handleConfDoorToggle(ConfDoorToggle pMessage)
    {
        GameObject obj = serviceLocator.interactableList[pMessage.ID];
        obj.GetComponent<DoorManager>().SetDoor(pMessage.isActivated);
    }

    private void handleConfActuatorToggle(ConfActuatorToggle pMessage)
    {
        Debug.Log("Got a actuator toggle with ID: " + pMessage.ID + "!");
        GameObject obj = serviceLocator.interactableList[pMessage.ID];
        switch (pMessage.obj)
        {
            case ConfActuatorToggle.Object.LEVER:
                Debug.Log("Its a lever toggle!");
                if (obj.GetComponent<Lever>().isActuated != pMessage.isActived)
                {
                    obj.GetComponent<Lever>().SetActivatedLever(pMessage.isActived);
                    //down = on, up = off
                    if (pMessage.isActived)
                    {
                        serviceLocator.GetFromList(ClientName).GetComponentInChildren<AnimationHandler>().DoTrigger("PullingLeverDown");
                    }
                    else
                    {
                        serviceLocator.GetFromList(ClientName).GetComponentInChildren<AnimationHandler>().DoTrigger("PullingLeverUp");
                    }
                }
                break;
            case ConfActuatorToggle.Object.PRESSUREPLATE:
                Debug.Log("Its a pressure plate toggle!");
                obj.GetComponent<PressurePlate>().UpdateActuator(pMessage.isActived);
                break;
            case ConfActuatorToggle.Object.BUTTON:
                Debug.Log("Its a button toggle!");
                obj.GetComponent<Button>().UpdateActuator(pMessage.isActived);
                if (pMessage.isActived)
                {
                    serviceLocator.GetFromList(ClientName).GetComponentInChildren<AnimationHandler>().DoTrigger("isPushingButton");
                }
                break;
            case ConfActuatorToggle.Object.CRACK:
                Debug.Log("Its a crack toggle!");
                obj.GetComponent<Crack>().FixCrack();
                break;
            default:
                Debug.LogError("ClientManager: Cannot handle actuator toggle!");
                break;
        }
    }
    private void handleConfMove(ConfMove pMessage)
    {
        //FindObjectOfType<BasicTCPClient>().handleConfMove(pMessage);
        GameObject player1 = serviceLocator.GetFromList("Player1");
        GameObject player2 = serviceLocator.GetFromList("Player2");

        if (pMessage.player == 0)
        {
            //player1.GetComponent<Movement>().moveToTile(new Vector3(pMessage.dirX, pMessage.dirY, pMessage.dirZ), pMessage.orientation);
            //player1.GetComponent<Movement>().moveToTile(new Vector3(pMessage.dirX, pMessage.dirY, pMessage.dirZ), pMessage.orientation);
            player1.GetComponent<Movement>().disectMovementCommands(pMessage.directions);
            if (pMessage.directions.Length < 5)player1.GetComponent<Movement>().SetRotation(new Vector3(pMessage.dirX, pMessage.dirY, pMessage.dirZ), pMessage.orientation);
            //player1.transform.rotation = Quaternion.Euler(0, 0, pMessage.orientation);

            //Debug.Log("Moved player 1!");
        }
        else
        {
            //player2.GetComponent<Movement>().moveToTile(new Vector3(pMessage.dirX, pMessage.dirY, pMessage.dirZ), pMessage.orientation);

            //player2.GetComponent<Movement>().moveToTile(new Vector3(pMessage.dirX, pMessage.dirY, pMessage.dirZ), pMessage.orientation);
            player2.GetComponent<Movement>().disectMovementCommands(pMessage.directions);
            if (pMessage.directions.Length < 5) player2.GetComponent<Movement>().SetRotation(new Vector3(pMessage.dirX, pMessage.dirY, pMessage.dirZ), pMessage.orientation);

            //player2.transform.rotation = Quaternion.Euler(0, 0, pMessage.orientation);

            //Debug.Log("Moved player 2!");
        }
    
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
                serviceLocator.GetFromList("SceneManager").GetComponent<SceneManagerScript>().LoadSceneSingle("Level0Tutorial-Town");

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

    private void handleBoxInfo(BoxInfo pHandleBox)
    {
        try
        {
            Debug.Log("Got a box with a value of" + pHandleBox.isPickedUp);
            serviceLocator.interactableList[pHandleBox.ID].GetComponent<BoxMovement>().UpdateBox(pHandleBox.isPickedUp, pHandleBox.posX, pHandleBox.posY, pHandleBox.posZ);
            if (pHandleBox.isPickedUp != isHoldingBox)
            {
                isHoldingBox = pHandleBox.isPickedUp;
                if (isHoldingBox)
                {
                    serviceLocator.GetFromList(ClientName).GetComponentInChildren<AnimationHandler>().DoTrigger("PickUp");
                }
                else if (!isHoldingBox)
                {
                    serviceLocator.GetFromList(ClientName).GetComponentInChildren<AnimationHandler>().DoTrigger("DropOff");
                }
            }
        }
        catch
        {
            Debug.LogError("Could not find box movement");
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
