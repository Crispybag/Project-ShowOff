using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{

    [SerializeField] [Range(0.1f, 1f)] private float minimumTime;
    [SerializeField] [Range(0.5f, 2f)] private float maximumTime;
    [SerializeField] [Range(0.01f, 0.2f)] private float minimumOffTimer;  
    [SerializeField] [Range(0.05f, 0.5f)] private float maximumOffTimer;  
    private float currentTimer;
    private float currentOffTimer;


    // Start is called before the first frame update
    void Start()
    {
        currentTimer = Random.Range(minimumTime, maximumTime);
        currentOffTimer = Random.Range(minimumOffTimer, maximumOffTimer);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTimer <= 0)
        {
            this.GetComponent<Light>().enabled = false;
            if(currentOffTimer <= 0)
            {
                currentTimer = Random.Range(minimumTime, maximumTime);
                currentOffTimer = Random.Range(minimumOffTimer, maximumOffTimer);
                this.GetComponent<Light>().enabled = true;
            }
            else
            {
                currentOffTimer -= Time.deltaTime;
            }
        }
        else
        {
            currentTimer -= Time.deltaTime;
        }
    }
}
