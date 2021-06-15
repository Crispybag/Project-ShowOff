using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private Animator animator;
    public bool isWalking;
    public bool isFalling;
    public List<string> idleAnimations;
    public bool isAbleToBox;
    public bool isAbleToCrawl;
    public bool isAbleToAir;
    public float timer;
    public float currentTimer;


    public void Start()
    {
        animator = this.transform.GetComponentInChildren<Animator>();
    }

    public void Update()
    {
        if (isWalking && !isFalling)
        {
            currentTimer = 0;
            animator.SetBool("isWalking", true);
            animator.SetBool("isFalling", false);
        }
        else if(!isWalking && !isFalling)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isFalling", false);
            if (currentTimer >= timer)
            {
                currentTimer = 0;
                timer = Random.Range(10, 20);
                if (idleAnimations.Count > 0)
                {
                    animator.SetTrigger(idleAnimations[Random.Range(0, idleAnimations.Count)]);
                }
            }
            else
            {
                currentTimer += Time.deltaTime;
            }
        }
        else if (isFalling)
        {
            animator.SetBool("isFalling", true);
        }

    }

    public void DoTrigger(string pTrigger)
    {
        try
        {
            switch (pTrigger)
            {
                case ("startCrawl"):
                    if (isAbleToCrawl)
                    {
                        animator.SetTrigger(pTrigger);
                    }
                    break;
                case ("stopCrawl"):
                    if (isAbleToCrawl)
                    {
                        animator.SetTrigger(pTrigger);
                    }
                    break;
                case ("startAir"):
                    if (isAbleToAir)
                    {
                        animator.SetTrigger(pTrigger);
                    }
                    break;
                case ("stopAir"):
                    if (isAbleToAir)
                    {
                        animator.SetTrigger(pTrigger);
                    }
                    break;
                default:
                    animator.SetTrigger(pTrigger);
                    break;
            }
            
        }
        catch
        {
            Debug.LogError("Given trigger name does not excist in the animator, name: " + pTrigger);
        }
    }



}
