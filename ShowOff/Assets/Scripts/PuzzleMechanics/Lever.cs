using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;

public class Lever : PuzzleFactory
{

    //AUTHOR: Ezra
    //SHORT DISCRIPTION:

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------


    //----------------------- private ------------------------

    [SerializeField] private Material _mat1;
    [SerializeField] private Material _mat2;
    [SerializeField] private float radius;
    private InputManager _inputManager;

    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================
    private void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = _mat1;
        _inputManager = serviceLocator.GetFromList("InputManager").GetComponent<InputManager>();
    }

    private void Update()
    {
        if (Vector3.Distance(this.gameObject.transform.position, serviceLocator.GetFromList("Player1").transform.position) < radius)
        {
            if (_inputManager.GetActionDown(InputManager.Action.ACT1))
            {
                if(this.GetComponentInChildren<InteractableTutorial>() != null)
                {
                    this.GetComponentInChildren<InteractableTutorial>().SetForcedDisabled();
                }
                ToggleMechanics();
                isActuated = !isActuated;
                if (isActuated)
                {
                    this.gameObject.GetComponent<MeshRenderer>().material = _mat2;
                }
                else
                {
                    this.gameObject.GetComponent<MeshRenderer>().material = _mat1;
                }
            }
        }
    }

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    public override void FinishMechanic()
    {
        throw new System.NotImplementedException();
    }


    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================

}