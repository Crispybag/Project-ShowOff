using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGameObject : MonoBehaviour
{
    public void ToggleGameObj(GameObject obj)
    {
        obj.SetActive(!obj.active);
    }
}
