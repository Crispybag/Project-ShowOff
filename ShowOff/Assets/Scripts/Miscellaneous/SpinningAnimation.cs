using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningAnimation : MonoBehaviour
{

    [SerializeField] [Range(0.1f, 2f)] private float minimumSpinSpeed;
    [SerializeField] [Range(0.5f, 3f)] private float maximumSpinSpeed;
    [SerializeField] [Range(1f, 5f)] private float minimumTimer;
    [SerializeField] [Range(3f, 10f)] private float maximumTimer;
    private float targetSpeed;
    private float currentSpeed;
    private float currentTimer;

    private Animator anim;

    private bool isGoingUp;

    // Start is called before the first frame update
    void Start()
    {
        targetSpeed = Random.Range(minimumSpinSpeed, maximumSpinSpeed);
        anim = GetComponent<Animator>();
        currentTimer = Random.Range(minimumTimer, maximumTimer);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentSpeed != targetSpeed)
        {
            if (isGoingUp)
            {
                currentSpeed += Time.deltaTime;
                if(targetSpeed - currentSpeed < 0.1f)
                {
                    currentSpeed = targetSpeed;
                }
            }
            else
            {
                currentSpeed -= Time.deltaTime;
                if (currentSpeed - targetSpeed < 0.1f)
                {
                    currentSpeed = targetSpeed;

                }
            }
            anim.speed = currentSpeed;
        }
        else
        {
            if (currentTimer <= 0)
            {
                targetSpeed = Random.Range(minimumSpinSpeed, maximumSpinSpeed);
                if (targetSpeed < currentSpeed)
                {
                    isGoingUp = false;
                }
                else
                {
                    isGoingUp = true;
                }
                currentTimer = Random.Range(minimumTimer, maximumTimer);
            }
            else
            {
                currentTimer -= Time.deltaTime;
            }
        }
    }
}
