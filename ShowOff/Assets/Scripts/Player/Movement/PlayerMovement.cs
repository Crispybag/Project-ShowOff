using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;
public class PlayerMovement : Movement
{
    [SerializeField] private float _moveSpeed;
    private InputManager _inputManager;
    public int playerPushWeight;


    private void Awake()
    {
        serviceLocator.AddToList("Player1", this.gameObject);
    }


    protected override void Start()
    {
        base.Start();
/*        canFall = true;
        _travelTime = _moveSpeed;*/
        _inputManager = serviceLocator.GetFromList("InputManager").GetComponent<InputManager>();
    }
    // Update is called once per frame

    protected override void Update()
    {
        base.Update();
        if (_inputManager.GetAction(InputManager.Action.HORIZONTAL))
        {
            moveToTile(new Vector3(_inputManager.getHorizontalInput(),0,0));
        }

        if (_inputManager.GetAction(InputManager.Action.VERTICAL))
        {
            moveToTile(new Vector3(0, 0, _inputManager.getVerticalInput()));
        }
        //this allows other objects to move this object. (Boxes now can move the player, useful for airchannels)
        if (wallCheckCalled)
        {
            moveToTile(_direction);
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

}
