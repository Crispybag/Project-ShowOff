using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using sharedAngy;
using System.Text;
using static ServiceLocator;

public class BasicTCPClient : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string _hostname = "localhost";
    [SerializeField] private string _username = "Poggywoggy";
    [SerializeField] private int _port = 42069;
    public TcpClient _client;
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


    [SerializeField] private GameObject player0;
    [SerializeField] private GameObject player1;
    public void handleConfMove(ConfMove pMoveConfirm)
    {
        Debug.Log("We got a confirmed movement for the player : " + pMoveConfirm.player);
        Debug.Log("With the following data: x: " + pMoveConfirm.dirX + " y: " + pMoveConfirm.dirY );
        if (pMoveConfirm.player == 0)
        {
            player0.GetComponent<Movement>().moveToTile(new Vector3(pMoveConfirm.dirX, pMoveConfirm.dirY, pMoveConfirm.dirZ) - player0.transform.position);
            //player0.transform.position = new Vector3(pMoveConfirm.dirX, pMoveConfirm.dirY, pMoveConfirm.dirZ);
            Debug.Log("Moved player 0!");
        }
        else
        {
            player1.GetComponent<Movement>().moveToTile(new Vector3(pMoveConfirm.dirX, pMoveConfirm.dirY, pMoveConfirm.dirZ) - player1.transform.position);
            //player1.transform.position = new Vector3(pMoveConfirm.dirX, pMoveConfirm.dirY, pMoveConfirm.dirZ);
            Debug.Log("Moved player 1!");
        }
    }

}
