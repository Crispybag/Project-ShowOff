using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ServiceLocator;
public class VolumeSettings : MonoBehaviour
{
    [SerializeField] Slider slider;
    AudioManager am;
    void Start()
    {
        am = serviceLocator.GetFromList("AudioManager").GetComponent<AudioManager>();
        slider.value = am.GetVolume();
    }

    // Update is called once per frame
    void Update()
    {
        am.SetVolume(slider.value);
    }
}
