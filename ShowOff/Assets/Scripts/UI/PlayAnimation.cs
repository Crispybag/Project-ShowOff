using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Start is called before the first frame update

    public Image _image;
    public Sprite[] spritesheet;
    public int currentSprite;
    public bool isButtonHovering = false;
    public int timer;
    private float currentTimer;

    public void OnPointerClick(PointerEventData eventData)
    {
        _image.sprite = spritesheet[0];
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isButtonHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isButtonHovering = false;
    }

    void Start()
    {
        _image.sprite = spritesheet[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (isButtonHovering)
        {
            if (currentTimer <= 0)
            {
                if (currentSprite < spritesheet.Length)
                {
                    currentSprite++;
                    _image.sprite = spritesheet[currentSprite - 1];
                }
                currentTimer = timer;
            }
            else
            {
                //60 = second
                currentTimer-= Time.deltaTime * 60;
            }
        }
        else
        {
            currentSprite = 0;
            currentTimer = 0;
            _image.sprite = spritesheet[0];
        }
    }
}
