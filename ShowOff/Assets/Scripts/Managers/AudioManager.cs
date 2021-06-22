using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection.Configuration;
using UnityEngine;
using static ServiceLocator;
public class AudioManager : MonoBehaviour
{
    static AudioManager am;
    Dictionary<string, FMOD.Studio.EventInstance> nonOverlappingSounds = new Dictionary<string, FMOD.Studio.EventInstance>();
    private float volumeParameter = 1.0f;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (null == am)
        {
            am = this;
            serviceLocator.AddToList("AudioManager", gameObject);
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Volume", volumeParameter);
            return;
        }

        Destroy(gameObject);

    }

    public void SetVolume(float value)
    {
        volumeParameter = value;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Volume", value);

    }

    public float GetVolume()
    {
        return volumeParameter;
    }


    public void AddToList(string indicatorName, string eventPath)
    {
        if (!isInList(indicatorName))
        {
            FMOD.Studio.EventInstance sound = FMODUnity.RuntimeManager.CreateInstance(eventPath);
            nonOverlappingSounds.Add(indicatorName, sound);
        }
        else
        {
            Debug.LogWarning("There is already a sound with key : '" + indicatorName + "' in the non overlapping sounds dictionary");
        }
    }

    public void PlayFromList(string indicatorName)
    {
        nonOverlappingSounds[indicatorName].start();
        //nonOverlappingSounds[indicatorName].release();
    }

    public void StopFromList(string indicatorName, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.IMMEDIATE)
    {
        nonOverlappingSounds[indicatorName].stop(stopMode);
    }

    public void setParamFromList(string indicatorName, string parameterName = "NoParameter", float pIndex = -1)
    {
        if (parameterName != "NoParameter")
        {
            nonOverlappingSounds[indicatorName].setParameterByName(parameterName, pIndex);
        }
    }


    private bool isInList(string indicatorName)
    {
        if (nonOverlappingSounds.ContainsKey(indicatorName))
            return true;
        else return false;
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

    public void PlaySound2D(string eventPath)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(eventPath, gameObject);
    }

}
