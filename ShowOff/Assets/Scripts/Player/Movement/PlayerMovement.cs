using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;
using sharedAngy;
using System.Net.Sockets;

/// <summary>
/// (Leo) Contains logic about the player movement
/// </summary>

public class PlayerMovement : Movement
{
    [SerializeField] private float _moveSpeed;
    

    [Tooltip("This will be the name in the service locator")]public string playerName = "Player1";

    private void Awake()
    {
        serviceLocator.AddToList(playerName, gameObject);
        _travelTime = _moveSpeed;
        //transform.Find("Crate").gameObject.SetActive(false);

    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }


}
