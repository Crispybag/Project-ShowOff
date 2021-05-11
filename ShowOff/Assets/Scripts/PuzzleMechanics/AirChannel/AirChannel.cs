using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirChannel : MonoBehaviour
{
    //AUTHOR: Ezra
    //SHORT DISCRIPTION:

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------

    public bool isAirEnabled = false;
    public List<GameObject> currentObjects = new List<GameObject>();

    //----------------------- private ------------------------

    [SerializeField] private float _airSpeed = 5;
    [SerializeField] private float _timer = 50;
    private float _currentTimer = 0;

    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================

    private void Start()
    {
        _currentTimer = _timer;
        Debug.Log(this.gameObject.transform.forward);
    }

    private void Update()
    {
        MoveObjects();
    }

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================

    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================

    private void MoveObjects()
    {
        if (isAirEnabled)
        {
            if (_currentTimer <= 0)
            {
                foreach (GameObject obj in currentObjects)
                {
                    //obj.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward);
                    obj.transform.position += this.gameObject.transform.forward;
                }
                _currentTimer = _timer;
            }
            _currentTimer--;
        }
    }

}