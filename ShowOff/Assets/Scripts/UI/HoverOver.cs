using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static ServiceLocator;
using sharedAngy;

public class HoverOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image characterImage;
    [SerializeField] private Sprite[] characterSprites;
    [Tooltip ("0 = Nuc, 1 = Alex")][SerializeField] private int playerIndex;
    private HandleCharacterSelection characterSelection;
    private bool isHovering = false;

    public void Start()
    {
        characterSelection = FindObjectOfType<HandleCharacterSelection>();
        if (characterSelection.currentActiveCharacter == playerIndex)
        {
            characterImage.sprite = characterSprites[0];
        }
        else
        {
            characterImage.sprite = characterSprites[2];
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        characterImage.sprite = characterSprites[1];
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        if (characterSelection.currentActiveCharacter == playerIndex)
        {
            characterImage.sprite = characterSprites[0];
        }
        else
        {
            characterImage.sprite = characterSprites[2];
        }
    }

    public void UpdateCharacters()
    {
        if (!isHovering)
        {
            if (characterSelection.currentActiveCharacter == playerIndex)
            {
                characterImage.sprite = characterSprites[0];
            }
            else
            {
                characterImage.sprite = characterSprites[2];
            }
        }
        else
        {
            characterImage.sprite = characterSprites[1];
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ReqPlayerSwitch playerSwitch = new ReqPlayerSwitch();
        playerSwitch.i = playerIndex;
        serviceLocator.GetFromList("ClientManager").GetComponent<ClientManager>().SendPackage(playerSwitch);
    }
}
