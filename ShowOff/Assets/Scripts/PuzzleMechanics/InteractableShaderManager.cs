using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InteractableShaderManager : MonoBehaviour
{

    private GameObject player;
    [SerializeField] private float radius;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void SetupShader(string playerName)
    {
        if (playerName == "Player1")
        {
            player = GameObject.FindGameObjectWithTag("Player1")/*.transform.GetChild(0).gameObject*/;
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player2");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(player.transform.position, this.gameObject.transform.position) <= radius)
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        else if(this.gameObject.GetComponent<MeshRenderer>().enabled == true)
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
