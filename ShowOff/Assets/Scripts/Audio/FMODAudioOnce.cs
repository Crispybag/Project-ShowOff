using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;
public class FMODAudioOnce : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string _eventPath;
    [SerializeField] private string _eventName;

    void Start()
    {
        AudioManager manager = serviceLocator.GetFromList("AudioManager").GetComponent<AudioManager>();
        manager.AddToList(_eventName, _eventPath);
        manager.PlayFromList(_eventName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
