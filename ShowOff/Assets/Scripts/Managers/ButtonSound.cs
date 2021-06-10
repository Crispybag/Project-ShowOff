using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static ServiceLocator;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler
{
    [FMODUnity.EventRef]
    [SerializeField] private string onButtonHover;
    
    [FMODUnity.EventRef]
    [SerializeField] private string onButtonClick;
    private AudioManager am;
    
    public void Start()
    {    
        am = serviceLocator.GetFromList("AudioManager").GetComponent<AudioManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        am.PlaySound2D(onButtonHover);
    }

    public void onButtonClicked()
    {
        am.PlaySound2D(onButtonClick);
    }
}