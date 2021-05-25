using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;
using sharedAngy;
using System.Net.Sockets;

public class PlayerMovement : Movement
{
    [SerializeField] private float _moveSpeed;
    private InputManager _inputManager;
    public int playerPushWeight;

    [Tooltip("This will be the name in the service locator!")]public string playerName = "Player1";
    [SerializeField] private BasicTCPClient basicClient;

    private void Awake()
    {
        serviceLocator.AddToList(playerName, gameObject);
        _travelTime = _moveSpeed;
    }
    protected override void Start()
    {
        base.Start();
    }
    // Update is called once per frame

    protected override void Update()
    {
        base.Update();
    }

    /*
    private Vector3 _direction;
    private bool wallCheckCalled;

    override public bool wallCheck(Vector3 pTargetPosition, Vector3 pCurrentPosition)
    {
        bool isWall = base.wallCheck(pTargetPosition, pCurrentPosition);
        if (!isWall)
        {
            wallCheckCalled = true;
            _direction = pTargetPosition - pCurrentPosition;
        }
        return isWall;
    }

    
    private void sendPackage(ASerializable pSerializable)
    {
        //create the packet
        Packet _outPacket = new Packet();
        _outPacket.Write(pSerializable);
        //send package to the stream
        StreamUtil.Write(basicClient._client.GetStream(), _outPacket.GetBytes());
    }
    */
}
