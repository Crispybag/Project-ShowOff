using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sharedAngy;
using static ServiceLocator;

public class BoxMovement : MonoBehaviour
{

    public int ID;

    private void Start()
    {
        serviceLocator.interactableList.Add(ID, this.gameObject);
    }

    public void UpdateBox(bool isPickedUp, int posX, int posY, int posZ)
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = !isPickedUp;
        this.gameObject.transform.position = new Vector3(posX, posY, posZ);
    }

}
