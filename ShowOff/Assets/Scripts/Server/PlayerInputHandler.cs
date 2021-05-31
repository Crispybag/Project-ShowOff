using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using sharedAngy;
using System.Text;
using static ServiceLocator;

public class PlayerInputHandler : MonoBehaviour
{
    private ClientManager _clientManager;
    
    void Start()
    {
        //connectToServer();
        _clientManager = serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>();
    }

    // Update is called once per frame
    void Update()
    {

        //receiveText();


        //Send Inputs
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("trying to send a package");
            ReqKeyDown keyDown = new ReqKeyDown();
            keyDown.keyInput = ReqKeyDown.KeyType.UP;
            _clientManager.SendPackage(keyDown);
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("trying to send a package");

            ReqKeyDown keyDown = new ReqKeyDown();
            keyDown.keyInput = ReqKeyDown.KeyType.DOWN;
            _clientManager.SendPackage(keyDown);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("trying to send a package");

            ReqKeyDown keyDown = new ReqKeyDown();
            keyDown.keyInput = ReqKeyDown.KeyType.LEFT;
            _clientManager.SendPackage(keyDown);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("trying to send a package");

            ReqKeyDown keyDown = new ReqKeyDown();
            keyDown.keyInput = ReqKeyDown.KeyType.RIGHT;
            _clientManager.SendPackage(keyDown);
        }        
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("trying to send a package");

            ReqKeyDown keyDown = new ReqKeyDown();
            keyDown.keyInput = ReqKeyDown.KeyType.INTERACTION;
            _clientManager.SendPackage(keyDown);
        }



        if(Input.GetKeyUp(KeyCode.UpArrow))
        {
            ReqKeyUp keyUp = new ReqKeyUp();
            keyUp.keyInput = ReqKeyUp.KeyType.UP;
            _clientManager.SendPackage(keyUp);

        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            ReqKeyUp keyUp = new ReqKeyUp();
            keyUp.keyInput = ReqKeyUp.KeyType.DOWN;
            _clientManager.SendPackage(keyUp);

        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            ReqKeyUp keyUp = new ReqKeyUp();
            keyUp.keyInput = ReqKeyUp.KeyType.LEFT;
            _clientManager.SendPackage(keyUp);

        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            ReqKeyUp keyUp = new ReqKeyUp();
            keyUp.keyInput = ReqKeyUp.KeyType.RIGHT;
            _clientManager.SendPackage(keyUp);
        }
    }

}
