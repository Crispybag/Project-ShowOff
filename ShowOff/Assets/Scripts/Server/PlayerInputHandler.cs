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
    private CameraManager _cameraManager;
    
    void Start()
    {
        //connectToServer();
        _clientManager = serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>();
        _cameraManager = serviceLocator.GetFromList("CameraManager").GetComponent<CameraManager>();
    }


    //dedtermines what direction the player should move based on camera position
    ReqKeyDown.KeyType determineKeyInput(KeyCode pKeyCode)
    {
        float keyAngle = 0;//key angle based on key input
        switch(pKeyCode)
        {
            case (KeyCode.UpArrow):
                keyAngle = 0;
                break;

            case (KeyCode.DownArrow):
                keyAngle = 180;
                break;

            case (KeyCode.LeftArrow):
                keyAngle = 270;
                break;

            case (KeyCode.RightArrow):
                keyAngle = 90;
                break;
            default:
                keyAngle = 0;
                Debug.Log("Lol bad code, need to flexible it");
                break;
        }
        keyAngle += _cameraManager.GetCameraRotation(); //add camera y-rotation
        keyAngle += 360; //remove negative numbers
        float finalAngle = keyAngle % 360; // make sure all numbers are between 0 and 360

        //determine final direction
        if (finalAngle <= 135 && finalAngle > 45) { return ReqKeyDown.KeyType.RIGHT; }
        else if (finalAngle <= 225 && finalAngle > 135) { return ReqKeyDown.KeyType.DOWN; }
        else if (finalAngle <= 315 && finalAngle > 225) { return ReqKeyDown.KeyType.LEFT; }
        else { return ReqKeyDown.KeyType.UP; }

    }

    ReqKeyUp.KeyType determineKeyInputUp(KeyCode pKeyCode)
    {
        float keyAngle = 0;//key angle based on key input
        switch (pKeyCode)
        {
            case (KeyCode.UpArrow):
                keyAngle = 0;
                break;

            case (KeyCode.DownArrow):
                keyAngle = 180;
                break;

            case (KeyCode.LeftArrow):
                keyAngle = 270;
                break;

            case (KeyCode.RightArrow):
                keyAngle = 90;
                break;
            default:
                keyAngle = 0;
                Debug.Log("Lol bad code, need to flexible it");
                break;
        }
        keyAngle += _cameraManager.GetCameraRotation(); //add camera y-rotation
        keyAngle += 360; //remove negative numbers
        float finalAngle = keyAngle % 360; // make sure all numbers are between 0 and 360

        //determine final direction
        if (finalAngle <= 135 && finalAngle > 45) { return ReqKeyUp.KeyType.RIGHT; }
        else if (finalAngle <= 225 && finalAngle > 135) { return ReqKeyUp.KeyType.DOWN; }
        else if (finalAngle <= 315 && finalAngle > 225) { return ReqKeyUp.KeyType.LEFT; }
        else { return ReqKeyUp.KeyType.UP; }

    }

    void sendReqKeyDown(KeyCode pKeyCode)
    {
        ReqKeyDown keyDown = new ReqKeyDown();
        keyDown.keyInput = determineKeyInput(pKeyCode);
        _clientManager.SendPackage(keyDown);
    }

    // Update is called once per frame
    void Update()
    {

        //receiveText();


        //Send Inputs
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            sendReqKeyDown(KeyCode.UpArrow);
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            sendReqKeyDown(KeyCode.DownArrow);

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            sendReqKeyDown(KeyCode.LeftArrow);

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            sendReqKeyDown(KeyCode.RightArrow);

        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("trying to send a package");

            ReqKeyDown keyDown = new ReqKeyDown();
            keyDown.keyInput = ReqKeyDown.KeyType.INTERACTION;
            _clientManager.SendPackage(keyDown);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ReqResetLevel resetLevel = new ReqResetLevel();
            resetLevel.wantsReset = true;
            _clientManager.SendPackage(resetLevel);
        }



        if(Input.GetKeyUp(KeyCode.UpArrow))
        {
            ReqKeyUp keyUp = new ReqKeyUp();
            keyUp.keyInput = determineKeyInputUp(KeyCode.UpArrow);
            _clientManager.SendPackage(keyUp);

        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            ReqKeyUp keyUp = new ReqKeyUp();
            keyUp.keyInput = determineKeyInputUp(KeyCode.DownArrow);
            _clientManager.SendPackage(keyUp);

        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            ReqKeyUp keyUp = new ReqKeyUp();
            keyUp.keyInput = determineKeyInputUp(KeyCode.LeftArrow);
            _clientManager.SendPackage(keyUp);

        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            ReqKeyUp keyUp = new ReqKeyUp();
            keyUp.keyInput = determineKeyInputUp(KeyCode.RightArrow);
            _clientManager.SendPackage(keyUp);
        }
    }

}
