using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;

/// <summary>
/// (Leo) Contains logic about the air channel mechanic
/// </summary>

public class AirChannelManager : MonoBehaviour
{
    public int ID;
    public bool isActuated;
    public List<GameObject> conditions = new List<GameObject>();

    void Start()
    {
        serviceLocator.interactableList.Add(ID, this.gameObject);
        ParticleSystem[] particlesystems = transform.parent.parent.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particlesystems)
        {
            var emission = particle.emission;
            emission.enabled = false;
        }
    }


}
