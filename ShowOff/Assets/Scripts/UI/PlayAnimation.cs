using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update

    public Image _image;
    public Sprite[] spritesheet;
    public int currentSprite;
    public bool isButtonHovering = false;
    public int timer;
    private int currentTimer;

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
                currentTimer--;
            }
        }
        else
        {
            currentSprite = 0;
            currentTimer = 0;
            _image.sprite = spritesheet[currentSprite];
        }
    }
}
