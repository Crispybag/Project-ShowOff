using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection.Configuration;
using UnityEngine;
using static ServiceLocator;
public class AudioManager : MonoBehaviour
{
    static AudioManager am;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (null == am)
        {
            am = this;
            serviceLocator.AddToList("AudioManager", gameObject);
            return;
        }

        Destroy(gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playSound(string eventPath, GameObject pObject, string parameterName = "NoParameter", int pIndex = -1 )
    {
        FMOD.Studio.EventInstance sound = FMODUnity.RuntimeManager.CreateInstance(eventPath);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(sound, pObject.transform, pObject.GetComponent<Rigidbody>());

        //Do this to set a certain parameter
        if (parameterName != "NoParameter")
        {
            sound.setParameterByName(parameterName, pIndex);
        }
        //Start thing
        sound.start();

        //Make sure it doesnt loop
        sound.release();

    }
}
