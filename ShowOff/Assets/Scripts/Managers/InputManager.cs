using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using static ServiceLocator;
public class InputManager : MonoBehaviour
{
    //AUTHOR: Leo Jansen
    //SHORT DISCRIPTION: Manages the input controls

    //HOW TO USE!
    // InputManager im = ServiceLocator.sl.GetFromList("InputManager").GetComponent<InputManager>();
    // im.GetAction();

    //=========================================================================================
    //                                     > Variables <
    //=========================================================================================

    //------------------------ public ------------------------
    public enum Action
    {
        UP = 0,
        RIGHT = 1,
        LEFT = 2,
        DOWN = 3,
        BACK = 4,
        ACT0 = 5,
        ACT1 = 6,
        HORIZONTAL = 7,
        VERTICAL = 8
    }

    //----------------------- private ------------------------
    [SerializeField] private KeyCode rightKey   = KeyCode.RightArrow;
    [SerializeField] private KeyCode leftKey    = KeyCode.LeftArrow;
    [SerializeField] private KeyCode upKey      = KeyCode.UpArrow;
    [SerializeField] private KeyCode downKey    = KeyCode.DownArrow;
    [SerializeField] private KeyCode backKey    = KeyCode.Z;
    [SerializeField] private KeyCode action0Key = KeyCode.X;
    [SerializeField] private KeyCode action1Key = KeyCode.C;

    //list of all taken keys
    private List<KeyCode> _occupiedKeys = new List<KeyCode>();

    //singleton reference
    private static InputManager _im;


    //=========================================================================================
    //                                   > Start/Update <
    //=========================================================================================

    private void Awake()
    {
        //singleton
        DontDestroyOnLoad(gameObject);
        if (null == _im)
        {
            _im = this;

            //add input manager to the service locator list
            if (null != serviceLocator)
            {
                serviceLocator.AddToList("InputManager", gameObject);
            }

            //fill the key list
            createKeyList();
            return;
        }
        Destroy(gameObject);

        
    }

    //=========================================================================================
    //                              > Public Tool Functions <
    //=========================================================================================
    
    /// <summary>
    /// Check if button is pressed at the moment
    /// </summary>
    /// <param name="pAction"> type of action that is happening right now</param>
    /// <returns> true if button is pressed </returns>
    public bool GetAction(Action pAction)
    {
        //match action with button
        switch(pAction)
        {
            case (Action.LEFT):
                if (Input.GetKey(leftKey))
                    return true;
                return false;
                
            case (Action.RIGHT):
                if (Input.GetKey(rightKey))
                    return true;
                return false;

            case (Action.UP):
                if (Input.GetKey(upKey))
                    return true;
                return false;

            case (Action.DOWN):
                if (Input.GetKey(downKey))
                    return true;
                return false;

            case (Action.BACK):
                if (Input.GetKey(backKey))
                    return true;
                return false;

            case (Action.ACT0):
                if (Input.GetKey(action0Key))
                    return true;
                return false;

            case (Action.ACT1):
                if (Input.GetKey(action1Key))
                    return true;
                return false;

            case (Action.HORIZONTAL):
                if (Input.GetKey(leftKey) || Input.GetKey(rightKey))
                    return true;
                return false;
            case (Action.VERTICAL):
                if (Input.GetKey(upKey) || Input.GetKey(downKey))
                    return true;
                return false;

            default:
                return false;
        }

    }
    
    /// <summary>
    /// Check if the button is released at the moment
    /// </summary>
    /// <param name="pAction"> type of action that is released </param>
    /// <returns> true if button is released </returns>
    public bool GetActionUp(Action pAction)
    {
        //match action with button
        switch (pAction)
        {
            case (Action.LEFT):
                if (Input.GetKeyUp(leftKey))
                    return true;
                return false;

            case (Action.RIGHT):
                if (Input.GetKeyUp(rightKey))
                    return true;
                return false;

            case (Action.UP):
                if (Input.GetKeyUp(upKey))
                    return true;
                return false;

            case (Action.DOWN):
                if (Input.GetKeyUp(downKey))
                    return true;
                return false;

            case (Action.BACK):
                if (Input.GetKeyUp(backKey))
                    return true;
                return false;

            case (Action.ACT0):
                if (Input.GetKeyUp(action0Key))
                    return true;
                return false;

            case (Action.ACT1):
                if (Input.GetKeyUp(action1Key))
                    return true;
                return false;

            case (Action.HORIZONTAL):
                if (Input.GetKeyUp(leftKey) || Input.GetKeyUp(rightKey))
                    return true;
                return false;

            case (Action.VERTICAL):
                if (Input.GetKeyUp(upKey) || Input.GetKeyUp(downKey))
                    return true;
                return false;

            default:
                return false;
        }

    }

    /// <summary>
    /// Check if the button is pressed down at the moment
    /// </summary>
    /// <param name="pAction">button that is pressed down</param>
    /// <returns>returns true if the button is pressed down at the moment </returns>
    public bool GetActionDown(Action pAction)
    {
        //match action with button
        switch (pAction)
        {
            case (Action.LEFT):
                if (Input.GetKeyDown(leftKey))
                    return true;
                return false;

            case (Action.RIGHT):
                if (Input.GetKeyDown(rightKey))
                    return true;
                return false;

            case (Action.UP):
                if (Input.GetKeyDown(upKey))
                    return true;
                return false;

            case (Action.DOWN):
                if (Input.GetKeyDown(downKey))
                    return true;
                return false;

            case (Action.BACK):
                if (Input.GetKeyDown(backKey))
                    return true;
                return false;

            case (Action.ACT0):
                if (Input.GetKeyDown(action0Key))
                    return true;
                return false;

            case (Action.ACT1):
                if (Input.GetKeyDown(action1Key))
                    return true;
                return false;

            case (Action.HORIZONTAL):
                if (Input.GetKeyDown(leftKey) || Input.GetKeyDown(rightKey))
                    return true;
                return false;

            case (Action.VERTICAL):
                if (Input.GetKeyDown(upKey) || Input.GetKeyDown(downKey))
                    return true;
                return false;

            default:
                return false;
        }
    }

    /// <summary>
    /// Sets a key to a certain action
    /// </summary>
    /// <param name="pKey">key you want to set it to</param>
    /// <param name="pAction">the action you want to set the key to</param>
    public void UpdateKeyCode(KeyCode pKey, Action pAction)
    {
        //check if there is a key already with the function

        if (!checkForKeyOccupation(pKey))
        {
            switch (pAction)
            {
                case (Action.LEFT):
                    leftKey = pKey;
                    return;

                case (Action.RIGHT):
                    rightKey = pKey;
                    return;

                case (Action.UP):
                    upKey = pKey;
                    return;

                case (Action.DOWN):
                    downKey = pKey;
                    return;

                case (Action.BACK):
                    backKey = pKey;
                    return;

                case (Action.ACT0):
                    action0Key = pKey;
                    return;

                case (Action.ACT1):
                    action1Key = pKey;
                    return;

                default:
                    return;
            }
        }
    }

    /// <summary>
    /// Returns the horizontal input
    /// </summary>
    /// <returns></returns>
    public int getHorizontalInput()
    {
        int _horizontalInput = 0;
        
        if (GetAction(Action.LEFT)) { _horizontalInput--; }
        if (GetAction(Action.RIGHT)) { _horizontalInput++; }
            
        return _horizontalInput;
    }
    public int getVerticalInput()
    {
        int _verticalInput = 0;

        if (GetAction(Action.DOWN)) { _verticalInput--; }
        if (GetAction(Action.UP)) { _verticalInput++; }

        return _verticalInput;
    }
    //=========================================================================================
    //                             > Private Tool Functions <
    //=========================================================================================
    /// <summary>
    /// checks if the key is already taken by another slot
    /// </summary>
    /// <param name="pKey">key that is going to be checked if it is already taken</param>
    /// <returns> true if the key is taken already </returns>
    private bool checkForKeyOccupation(KeyCode pKey)
    {
        foreach (KeyCode key in _occupiedKeys)
        {
            if (key == pKey) return true;
        }
        return false;
    }

    /// <summary>
    /// create the key list on first run. Add all the keys to the list
    /// </summary>
    private void createKeyList()
    {
        _occupiedKeys.Add(leftKey);
        _occupiedKeys.Add(rightKey);
        _occupiedKeys.Add(upKey);
        _occupiedKeys.Add(downKey);
        _occupiedKeys.Add(backKey);
        _occupiedKeys.Add(action0Key);
        _occupiedKeys.Add(action1Key);
    }
}
