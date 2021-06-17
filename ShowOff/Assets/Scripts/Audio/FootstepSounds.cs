using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FootstepSounds : MonoBehaviour
{
    //=======================================================================================
    //                            >  Components & Variables  <
    //=======================================================================================

    // public objects
    [Header("Components")]
    [FMODUnity.EventRef] public string footStepsEventPath;
    public string materialParameter = "none";
    public string parameterOcclusion = "none";
    public float rayDistance = 1.2f;
    //public string[] materialTypes;
    [HideInInspector] public int defaultMaterial;

    private RaycastHit hit;
    private int _materialValue;


    //Setting up occlusion
    private GameObject _player;
    //Power of occlusion
    [Range(0f, 1f)]
    public float volume = 0.7f;

    //Layermask to hide certain objects
    public LayerMask occlusionLayer = 1;

    //=======================================================================================
    //                              >  Start And Update  <
    //=======================================================================================

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }


    //=======================================================================================
    //                              >  Update Functions <
    //=======================================================================================
    //-------------------------------------MaterialCheck-----------------------------------------
    //Description of function 
    public void MaterialCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
        {
            if (hit.collider.gameObject.GetComponent<FMODStudioMaterialSetter>())
            {
                _materialValue = hit.collider.gameObject.GetComponent<FMODStudioMaterialSetter>().materialValue;
            }
            else _materialValue = defaultMaterial;
        }
        else _materialValue = defaultMaterial;

    }
    //--------------------------------------PlayFootStep-----------------------------------------
    //Description of function 
    public void PlayFootStep()
    {
        MaterialCheck();
        FMOD.Studio.EventInstance footstep = FMODUnity.RuntimeManager.CreateInstance(footStepsEventPath);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(footstep, transform, GetComponent<Rigidbody>());
        if (materialParameter != "none") footstep.setParameterByName(materialParameter, _materialValue);
        footstep.start();
        footstep.release();


        if (parameterOcclusion != "none")
        {
            //Check for occlusion
            RaycastHit hit;
            Physics.Linecast(transform.position, _player.transform.position, out hit, occlusionLayer);

            if (hit.collider.tag == "Player")
            {
                footstep.setParameterByName(parameterOcclusion, 1f);
            }

            else
            {
                footstep.setParameterByName(parameterOcclusion, volume);

            }
        }
    }
}