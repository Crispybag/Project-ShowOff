using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;


/// <summary>
/// (Ezra) Contains logic about the crack mechanic
/// </summary>

public class Crack : Actuators
{

    [SerializeField] private ParticleSystem[] particlesTurnOffStart;

    public void Start()
    {
        serviceLocator.interactableList.Add(ID, this.gameObject);
        foreach (ParticleSystem particle in particlesTurnOffStart)
        {
            var emission = particle.emission;
            emission.enabled = false;
        }
    }

    public void FixCrack()
    {
        this.transform.parent.GetComponentInChildren<CrackAnimation>().fixing = true;
        foreach(ParticleSystem particle in particlesTurnOffStart)
        {
            var emission = particle.emission;
            emission.enabled = true;
        }
    }

}
