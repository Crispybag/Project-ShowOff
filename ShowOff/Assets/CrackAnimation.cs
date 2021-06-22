using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrackAnimation : MonoBehaviour
{

    private Material mat;
    public Sprite[] sprites;
    [SerializeField] private GameObject[] objectsToDisable;
    private int currentSprite = 0;
    private float timer;
    [SerializeField] private float maxTimer = 0.25f;
    [HideInInspector] public bool fixing =false;

    // Start is called before the first frame update
    void Start()
    {
        mat = this.transform.GetComponent<MeshRenderer>().material;
        mat.SetTexture("_BaseMap", sprites[currentSprite].texture);
    }


    private void Update()
    {
        if (fixing)
        {
            playAnimation();
        }
    }

    public void playAnimation()
    {
        //Texture tex =  mat.GetTexture("_BaseMap");
        if (timer > maxTimer)
        {
            if (currentSprite >= sprites.Length)
            {
                currentSprite = 0;
                foreach(GameObject obj in objectsToDisable)
                {
                    Destroy(obj);
                }
                Destroy(this.gameObject);
            }
            mat.SetTexture("_BaseMap", sprites[currentSprite].texture);
            currentSprite++;
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }


}
