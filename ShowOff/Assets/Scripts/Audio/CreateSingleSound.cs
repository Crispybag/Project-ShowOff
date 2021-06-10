using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;
public class CreateSingleSound : MonoBehaviour
{
    [FMODUnity.EventRef]
    [SerializeField] private string eventPath;

    private void Start()
    {
        AudioManager am = serviceLocator.GetFromList("AudioManager").GetComponent<AudioManager>();
        am.AddToList(eventPath, eventPath);
    }
}
