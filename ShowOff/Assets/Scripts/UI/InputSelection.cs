using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InputSelection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject otherInput;
    [SerializeField] private Sprite[] spritesheet;
    [SerializeField] private bool isSelected;


    public void Start()
    {
        if (!isSelected)
        {
            this.GetComponent<Image>().sprite = spritesheet[0];
        }
        else
        {
            this.GetComponent<Image>().sprite = spritesheet[2];
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        this.GetComponent<Image>().sprite = spritesheet[1];
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelected)
        {
            this.GetComponent<Image>().sprite = spritesheet[2];
        }
        else
        {
            this.GetComponent<Image>().sprite = spritesheet[0];
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        otherInput.GetComponent<Image>().sprite = otherInput.GetComponent<InputSelection>().spritesheet[0];
        isSelected = true;
        otherInput.GetComponent<InputSelection>().isSelected = false;
    }
}
