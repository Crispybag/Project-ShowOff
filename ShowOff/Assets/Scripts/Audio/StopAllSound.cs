using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;
public class StopAllSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        serviceLocator.GetFromList("AudioManager").GetComponent<AudioManager>().cancelAllContinuousSounds();
    }
}
