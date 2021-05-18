using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ServiceLocator;

public class SceneManagerScript : MonoBehaviour
{
    //AUTHOR: Ezra
    //SHORT DISCRIPTION: Handles all scenes, make sure they can switch etc.

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------


    //----------------------- private ------------------------



    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================

    private void Awake()
    {
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

    

    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================



}