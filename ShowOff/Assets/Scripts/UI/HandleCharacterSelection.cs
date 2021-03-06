using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// (Ezra) Handles incomming message from server to update the character selection
/// </summary>

public class HandleCharacterSelection : MonoBehaviour
{
    [SerializeField] private GameObject NucDeselect;
    [SerializeField] private GameObject NucSelect;
    [SerializeField] private GameObject AlexDeselect;
    [SerializeField] private GameObject AlexSelect;
    [HideInInspector] public int currentActiveCharacter = 0;
    public void UpdateCharacters(int currentSelectedCharacter)
    {
        currentActiveCharacter = currentSelectedCharacter;
        foreach (HoverOver hover in FindObjectsOfType<HoverOver>())
        {
            hover.UpdateCharacters();
        }
    }


}
