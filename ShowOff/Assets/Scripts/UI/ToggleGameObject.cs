using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// (Ezra) Toggles gameobject
/// </summary>

public class ToggleGameObject : MonoBehaviour
{
    public void ToggleGameObj(GameObject obj)
    {
        obj.SetActive(!obj.active);
    }
}
