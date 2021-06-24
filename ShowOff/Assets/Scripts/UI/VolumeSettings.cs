using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ServiceLocator;
public class VolumeSettings : MonoBehaviour
{
    [SerializeField] Slider soundSlider;
    [SerializeField] Slider volumeSlider;
    AudioManager am;
    void Start()
    {
        am = serviceLocator.GetFromList("AudioManager").GetComponent<AudioManager>();
        soundSlider.value = am.GetVolume();
        volumeSlider.value = am.GetVolume(1);
    }

    // Update is called once per frame
    void Update()
    {
        am.SetVolume(soundSlider.value);
        am.SetVolume(volumeSlider.value, 1);
    }
}
