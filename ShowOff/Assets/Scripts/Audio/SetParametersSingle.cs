using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;

/// <summary>
/// (Leo) Sets fmod event paramater from singleton
/// </summary>

public class SetParametersSingle : MonoBehaviour
{
    public enum ActivationRequirement
    { 
    START,
    COLLISIONENTER,
    COLLISIONEXIT
    }




    [SerializeField] private ActivationRequirement activationReq;
    [SerializeField] private string collisionTag = "none";
    
    [SerializeField] private List<FMODParameter> parameters;
    private void setParameters()
    {
        AudioManager manager = serviceLocator.GetFromList("AudioManager").GetComponent<AudioManager>();

        foreach (FMODParameter param in parameters)
        {
            manager.setParamFromList(param.eventPath, param.parameterName, param.value);
        }
    }


    private void Start()
    {
        if (activationReq == ActivationRequirement.START) 
        { 
            setParameters(); 
        
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (activationReq == ActivationRequirement.COLLISIONENTER && collision.gameObject.tag == collisionTag) { setParameters(); }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (activationReq == ActivationRequirement.COLLISIONEXIT && collision.gameObject.tag == collisionTag) { setParameters(); }
    }
}
