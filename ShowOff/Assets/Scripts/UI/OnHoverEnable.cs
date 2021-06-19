using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/// <summary>
/// (Ezra) When hovering over an object it will be slowly enabled
/// </summary>

public class OnHoverEnable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private Image image;
    [SerializeField] private Sprite character;

    [SerializeField] private FadeManager fadeManager;


    public void OnPointerEnter(PointerEventData eventData)
    {
        fadeManager.image.sprite = character;
        fadeManager.fadingIn = true;
        fadeManager.fadingOut = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        fadeManager.fadingIn = false;
        fadeManager.fadingOut = true;
    }

}
