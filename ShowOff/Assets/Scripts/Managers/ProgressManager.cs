using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;

public delegate void ProgressHandler();
public class ProgressManager : MonoBehaviour
{
    //AUTHOR: Leo Jansen
    //SHORT DISCRIPTION: Keeps track of how far the player is in the level to guide later or give some simple UI indication on how far the player is.

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------
    public ProgressHandler onProgressMade;


    //----------------------- private ------------------------

    //singleton
    private static ProgressManager _pm;
    
    //Amount of intended steps the player has taken
    private int _playerProgress;
    
    //Amount of puzzle steps
    private int _puzzleSteps;

    //Amount of completion in percentages
    private int _percentageCompletion;
    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================
    private void Awake()
    {
        //singleton
        DontDestroyOnLoad(gameObject);
        if (null == _pm)
        {
            _pm = this;

            //add progress manager to the service locator list
            if (null != serviceLocator)
            {
                serviceLocator.AddToList("ProgressManager", gameObject);
            }
            return;
        }
        Destroy(gameObject);

    }

    private void Start()
    {
        //add calculating to making progress
        onProgressMade += calculatePercentage;
    }

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================
    
    /// <summary>
    /// Set the amount of steps the current puzzle will take
    /// </summary>
    /// <param name="pSteps"> amount of steps</param>
    public void SetPuzzleSteps(int pSteps)
    {
        _puzzleSteps = pSteps;
    }

    /// <summary>
    /// Call when the player completes a part of the puzzle in the right order
    /// </summary>
    public void MakeProgress()
    {
        if (onProgressMade != null)
        {
            onProgressMade();
        }
    }

    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================
    /// <summary>
    /// Calculate the percentage of the puzzle completion 
    /// </summary>
    private void calculatePercentage()
    {
        _percentageCompletion = (int)(( (float)_playerProgress / (float)_puzzleSteps) * 100);
    }
}
