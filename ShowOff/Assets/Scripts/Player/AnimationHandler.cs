using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private Animator animator;
    public bool isWalking;
    public bool isFalling;
    private bool isCrawling;
    public List<string> idleAnimations;
    public bool isAbleToBox;
    public bool isAbleToCrawl;
    public bool isAbleToAir;
    public float timer;
    public float currentTimer;

    private float fallTimer;

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
        }
        else if(!isWalking && !isFalling)
        {
            animator.SetBool("isWalking", false);
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
            
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 1))
            {
                fallTimer += Time.deltaTime;
                if(fallTimer > 0.25f)
                {
                    fallTimer = 0;
                    DoTrigger("isLanding");
                    isFalling = false;
                }
            }
        }
    }

    public AnimationClip GetAnimatorClipInfos()
    {
        AnimatorClipInfo[] pog = animator.GetCurrentAnimatorClipInfo(0);
        return pog[0].clip;

    }
    public void DoTrigger(string pTrigger)
    {
        try
        {
            switch (pTrigger)
            {
                case ("startCrawling"):
                    //isCrawling is fail save, so if it gets bugged out and gets another packet, it wont do the trigger if you are already crawling
                    if (!isCrawling)
                    {
                        if (isAbleToCrawl)
                        {
                            animator.SetTrigger(pTrigger);
                            isCrawling = true;
                        }
                    }
                    break;
                case ("stopCrawling"):
                    if (isCrawling)
                    {
                        if (isAbleToCrawl)
                        {
                            animator.SetTrigger(pTrigger);
                            isCrawling = false;
                        }
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
