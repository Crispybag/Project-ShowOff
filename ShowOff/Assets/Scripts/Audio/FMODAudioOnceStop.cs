using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;
public class FMODAudioOnceStop : MonoBehaviour
{
    // Start is called before the first frame update
    [FMODUnity.EventRef]
    [SerializeField]string eventPath;
    void Start()
    {
        serviceLocator.GetFromList("AudioManager").GetComponent<AudioManager>().StopFromList(eventPath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
