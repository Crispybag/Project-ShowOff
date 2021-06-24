using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;

public class PlaySingleSound2 : MonoBehaviour
{
    [FMODUnity.EventRef]
    [SerializeField] private string eventPath;
    AudioManager am;
    private void Start()
    {
        am = serviceLocator.GetFromList("AudioManager").GetComponent<AudioManager>();
    }

    public void PlaySoundOnce2(int lol)
    {
        am.playSound(eventPath, gameObject);
    }
}
