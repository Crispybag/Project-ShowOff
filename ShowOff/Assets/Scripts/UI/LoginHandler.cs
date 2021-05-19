using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using sharedAngy;
using static ServiceLocator;

public class LoginHandler : MonoBehaviour
{

    [SerializeField] private int _port = 42069;
    [SerializeField] private string _hostname = "localhost";
    [SerializeField] private string _username = "Guest";
    [SerializeField] private InputField _ipAdressInput;
    [SerializeField] private InputField _userNameInput;
    [SerializeField] private Text _feedbackText;

    private void Start()
    {
        _feedbackText.text = "";
        _ipAdressInput.text = "localhost";
        _userNameInput.text = "Guest1";
    }
    public void ConnectToServer()
    {
        _hostname = _ipAdressInput.text;
        _username = _userNameInput.text;

        try
        {

            FindObjectOfType<ClientManager>().client = new TcpClient();
            //connect to the ip
            FindObjectOfType<ClientManager>().client.Connect(_hostname, _port);
            ReqJoinServer joinRequest = new ReqJoinServer();
            joinRequest.requestedName = _username;
            FindObjectOfType<ClientManager>().SendPackage(joinRequest);
        }
        catch
        {
            Debug.LogError("could not connect client to server");
        }
    }
    public void handleConfJoin(ConfJoinServer pJoinConfirm)
    {
        if (pJoinConfirm.acceptStatus)
        {
            Debug.Log("YAY :D, You connected");
            serviceLocator.GetFromList("SceneManager").GetComponent<SceneManagerScript>().LoadSceneSingle("Lobby");
        }
        else
        {
            _feedbackText.text = pJoinConfirm.message;
            Debug.Log("Not Accepted, same name probably");
        }
    }

}
