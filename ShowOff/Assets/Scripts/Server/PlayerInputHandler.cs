using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using sharedAngy;
using System.Text;
using static ServiceLocator;
using UnityEngine.SceneManagement;

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
    int determineKeyInput(KeyCode pKeyCode)
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
        keyAngle = _cameraManager.GetCameraRotation(); //add camera y-rotation
        keyAngle += 360; //remove negative numbers
        float finalAngle = keyAngle % 360; // make sure all numbers are between 0 and 360

        //determine final direction
        if (finalAngle <= 135 && finalAngle > 45) { return 90; }
        else if (finalAngle <= 225 && finalAngle > 135) { return 180; }
        else if (finalAngle <= 315 && finalAngle > 225) { return 270; }
        else { return 0; }

    }


    void sendReqKeyDown(KeyCode pKeyCode, ReqKeyDown.KeyType type)
    {
        ReqKeyDown keyDown = new ReqKeyDown();
        keyDown.keyInput = type;
        keyDown.rotation = determineKeyInput(pKeyCode);
        _clientManager.SendPackage(keyDown);
    }

    // Update is called once per frame
    void Update()
    {

        //receiveText();


            //Send Inputs

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            foreach(Scene scene in SceneManager.GetAllScenes())
            {
                if(scene.name == "Options")
                {
                    SceneManager.UnloadSceneAsync("Options");
                    return;
                }
            }
            SceneManager.LoadScene("Options", LoadSceneMode.Additive);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            sendReqKeyDown(KeyCode.UpArrow, ReqKeyDown.KeyType.UP);
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            sendReqKeyDown(KeyCode.DownArrow, ReqKeyDown.KeyType.DOWN);

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            sendReqKeyDown(KeyCode.LeftArrow, ReqKeyDown.KeyType.LEFT);

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            sendReqKeyDown(KeyCode.RightArrow, ReqKeyDown.KeyType.RIGHT);

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
            resetLevel.sceneName = serviceLocator.GetFromList("SceneManager").GetComponent<SceneManagerScript>().getCurrentScene();
            _clientManager.SendPackage(resetLevel);
        }



        if (Input.GetKeyUp(KeyCode.UpArrow))
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
