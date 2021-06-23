using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateBoxMesh : MonoBehaviour
{
    [SerializeField] private GameObject crate;
    [SerializeField] private GameObject crackTool;


    private void Start()
    {
        crate.SetActive(false);
        crackTool.SetActive(false);
    }

    public void DisableBox()
    {
        crate.SetActive(false);
    }

    public void EnableBox()
    {
        crate.SetActive(true);
    }
    public void DisableCrackTool()
    {
        crackTool.SetActive(false);
    }

    public void EnableCrackTool()
    {
        crackTool.SetActive(true);
    }

}
