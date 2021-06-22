using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnAndOff : MonoBehaviour
{
    [FMODUnity.EventRef]
    [SerializeField]private string eventPath;

    FMOD.Studio.EventInstance sound;
    void Start()
    {
        sound = FMODUnity.RuntimeManager.CreateInstance(eventPath);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(sound, gameObject.transform, gameObject.GetComponent<Rigidbody>());
    }

    public void StartSound()
    {
        FMOD.Studio.PLAYBACK_STATE state;
        sound.getPlaybackState(out state);
        if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            sound.start();
        }
    }

    public void StopSound()
    {
        sound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
