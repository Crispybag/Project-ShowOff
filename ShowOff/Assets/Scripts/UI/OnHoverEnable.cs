using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnHoverEnable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private Image image;
    [SerializeField] private Sprite character;

    [SerializeField] private FadeManager fadeManager;


    public void OnPointerEnter(PointerEventData eventData)
    {
        /*        Color newColor = image.color;
                newColor.a = 1;
                image.color = newColor;*/
        fadeManager.image.sprite = character;
        fadeManager.fadingIn = true;
        fadeManager.fadingOut = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        /*        Color newColor = image.color;
                newColor.a = 0;
                image.color = newColor;*/
        fadeManager.fadingIn = false;
        fadeManager.fadingOut = true;
    }

/*    private void Update()
    {
        FadeIn();
        FadeOut();
    }

    private void FadeIn()
    {
        if (fadingIn)
        {
            if (image.color.a < 1)
            {
                Debug.Log("Not equal to 1");
                Color newColor = image.color;
                newColor.a += fadeSpeed;
                if (newColor.a > 1)
                {
                    newColor.a = 1;
                    image.color = newColor;
                    fadingIn = false;
                }
                image.color = newColor;
            }
        }
    }

    private void FadeOut()
    {
        if (fadingOut)
        {
            if (image.color.a > 0)
            {
                Color newColor = image.color;
                newColor.a -= fadeSpeed;
                if (newColor.a <= 0)
                {
                    newColor.a = 0;
                    image.color = newColor;
                    fadingOut = false;
                }
                image.color = newColor;
            }
        }
    }*/


}
