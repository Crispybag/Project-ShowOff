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
        serviceLocator.AddToList(playerName, this.gameObject);
    }


    protected override void Start()
    {
        base.Start();
        _inputManager = serviceLocator.GetFromList("InputManager").GetComponent<InputManager>();
    }
    // Update is called once per frame

    protected override void Update()
    {
        base.Update();
        if (_inputManager.GetAction(InputManager.Action.HORIZONTAL))
        {
            ReqMove newMove = new ReqMove();
            newMove.name = playerName;
            newMove.dirX = _inputManager.getHorizontalInput();
            newMove.dirY = 0;
            newMove.dirZ = 0;
            sendPackage(newMove);
            //moveToTile(new Vector3(_inputManager.getHorizontalInput(),0,0));
        }

        if (_inputManager.GetAction(InputManager.Action.VERTICAL))
        {
            ReqMove newMove = new ReqMove();
            newMove.name = playerName;
            newMove.dirX = 0;
            newMove.dirY = 0;
            newMove.dirZ = _inputManager.getVerticalInput();
            sendPackage(newMove);
            //moveToTile(new Vector3(0, 0, _inputManager.getVerticalInput()));
        }
        //this allows other objects to move this object. (Boxes now can move the player, useful for airchannels)
        if (wallCheckCalled)
        {
            //moveToTile(_direction);
            ReqMove newMove = new ReqMove();
            newMove.name = playerName;
            newMove.dirX = (int)_direction.x;
            newMove.dirY = (int)_direction.y;
            newMove.dirZ = (int)_direction.z;
            sendPackage(newMove);
        }
        wallCheckCalled = false;

    }

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
}
