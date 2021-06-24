using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ServiceLocator;
using sharedAngy;


/// <summary>
/// (Ezra) Handles the options menu, with specific functions
/// </summary>

public class Options : MonoBehaviour
{
    [SerializeField] private Text optionText;
    
    void Start()
    {
        serviceLocator.AddToList("Options", this.gameObject);
        UpdateText();
    }

    private bool wantToReset = false;

    public void RequestResetLevel()
    {
        ReqResetLevel resetLevel = new ReqResetLevel();
        wantToReset = !wantToReset;
        resetLevel.wantsReset = wantToReset;
        resetLevel.sceneName = serviceLocator.GetFromList("SceneManager").GetComponent<SceneManagerScript>().getCurrentScene();
        serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().SendPackage(resetLevel);
    }

    public void UpdateText()
    {
        optionText.text = "Reset Level ( " + serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().playersWantToReset + " / 2 )";
    }

}
