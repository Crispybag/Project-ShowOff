using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;
using sharedAngy;
using System.Net.Sockets;

public class PlayerMovement : Movement
{
    [SerializeField] private float _moveSpeed;

    [Tooltip("This will be the name in the service locator!")]public string playerName = "Player1";

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


}
