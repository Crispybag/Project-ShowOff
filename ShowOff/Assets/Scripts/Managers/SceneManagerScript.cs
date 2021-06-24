using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ServiceLocator;
using sharedAngy;

public class SceneManagerScript : MonoBehaviour
{
    //AUTHOR: Ezra
    //SHORT DISCRIPTION: Handles all scenes, make sure they can switch etc.

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------


    //----------------------- private ------------------------
    private static SceneManagerScript _sceneManager;


    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        //if (null == _sceneManager)
        //{
         //   _sceneManager = this;
          //  return;
        //}
        //Destroy(this.gameObject);

        serviceLocator.AddToList("SceneManager", this.gameObject);
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    public void LoadSceneSingle(string sceneName)
    {
        serviceLocator.interactableList.Clear();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void LoadSceneAdditive(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void UnLoadSceneAdditive(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }

    public void EnableGameobject(GameObject pObject)
    {
        pObject.SetActive(true);
    }
    
    public void DisableGameobject(GameObject pObject)
    {
        pObject.SetActive(false);
    }

    public void ToggleGameobject(GameObject pObject)
    {
        pObject.SetActive(!pObject.activeSelf);
        
    }

    public void RequestRoom(int roomNumber)
    {
        switch(roomNumber)
        {
            case 0: //login
                ReqJoinRoom newLoginRequest = new ReqJoinRoom();
                newLoginRequest.room = ReqJoinRoom.Rooms.LOGIN;
                serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().SendPackage(newLoginRequest);
                break;
            case 1: //lobby
                ReqJoinRoom newLobbyRequest = new ReqJoinRoom();
                newLobbyRequest.room = ReqJoinRoom.Rooms.LOBBY;
                serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().SendPackage(newLobbyRequest);
                break;
            case 2: //game
                ReqJoinRoom newGameRequest = new ReqJoinRoom();
                newGameRequest.room = ReqJoinRoom.Rooms.GAME;
                serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().SendPackage(newGameRequest);
                break;
            default:
                Debug.LogError("No handler for such given number!");
                break;
        }
    }


    

    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================



}