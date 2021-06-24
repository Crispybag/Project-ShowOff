using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{

    [SerializeField] public Image image;


    [SerializeField] public bool fadingIn = false;
    [SerializeField] public bool fadingOut = false;

    [SerializeField] private float fadeSpeed;

    private void Start()
    {
        Color newColor = image.color;
        newColor.a = 0;
        image.color = newColor;
    }


    private void Update()
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
    }
}
