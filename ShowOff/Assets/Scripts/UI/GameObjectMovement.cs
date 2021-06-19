using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServiceLocator;


/// <summary>
/// (Ezra) Moves a gameobject accordingly, mostly used in main menu
/// </summary>


public class GameObjectMovement : MonoBehaviour
{

    [SerializeField] private GameObject objecToMove;
    private Vector3 _currentPosition;
    private Vector3 _targetPosition;

    private Vector3 currentScale;
    private Vector3 targetScale;

    [SerializeField]private float _travelTime = 1f;
    private float timer;

    public bool canMove = true;
    private bool tryingToConnectToLobby = false;


    // Start is called before the first frame update
    void Start()
    {
        _currentPosition = objecToMove.transform.position;
        _targetPosition = objecToMove.transform.position;

        currentScale = objecToMove.transform.localScale;
        targetScale = objecToMove.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //lerp position
        timer += Time.deltaTime;
        float ratio = timer / _travelTime;

        objecToMove.transform.position = Vector3.Lerp(_currentPosition, _targetPosition, ratio);
        objecToMove.transform.localScale = Vector3.Lerp(currentScale, targetScale, ratio);
        checkForMovement();
    }

    protected void checkForMovement()
    {
        //makes sure we dont move multiple tiles within the same amount of time
        //because when holding the button id would stack if we wouldnt do this.
        canMove = false;

        if ((_targetPosition - objecToMove.transform.position).magnitude < 0.01f && (targetScale - objecToMove.transform.localScale).magnitude < 0.01f)
        {
            canMove = true;
            objecToMove.transform.position = _targetPosition;
            _currentPosition = objecToMove.transform.position;

            objecToMove.transform.localScale = targetScale;
            currentScale = objecToMove.transform.localScale;


            if (tryingToConnectToLobby)
            {
                serviceLocator.GetFromList("SceneManager").GetComponent<SceneManagerScript>().LoadSceneSingle("Lobby");
            }
        }
    }
    public void MoveObjectHorizontal(float direction)
    {
        if (canMove)
        {
            _currentPosition = objecToMove.transform.position;
            _targetPosition.x = _currentPosition.x + direction;
            timer = 0;
        }
    }

    public void MoveObjectForwardToGoToLobby()
    {
        Debug.Log("Trying to move to lobby ");
        if (canMove)
        {
            currentScale = objecToMove.transform.localScale;
            targetScale =  currentScale * 3;
            tryingToConnectToLobby = true;
            timer = 0;
        }
    }

}
