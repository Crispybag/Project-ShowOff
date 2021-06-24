using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static ServiceLocator;

/// <summary>
/// (Ezra) Plays an animation by spritesheet when being hovered over
/// </summary>
public class PlayAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Start is called before the first frame update

    public Image _image;
    public Sprite[] spritesheet;
    public int currentSprite;
    public bool isButtonHovering = false;
    public int timer;
    private float currentTimer;

    [FMODUnity.EventRef]
    [SerializeField]private string _eventPath = "event:/UI-UX/spray hover";
    [FMODUnity.EventRef]
    [SerializeField] private string _clickSound = "event:/UI-UX/pressed button";

    private AudioManager am;
    private FMOD.Studio.EventInstance _hoverSound;

    public void OnPointerClick(PointerEventData eventData)
    {
        _image.sprite = spritesheet[0];
        am.PlaySound2D(_clickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isButtonHovering = true;
        _hoverSound.start();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isButtonHovering = false;
        _hoverSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void OnEnable()
    {
        isButtonHovering = false;
        _image.sprite = spritesheet[0];
    }

    void Start()
    {
        _image.sprite = spritesheet[0];
        am = serviceLocator.GetFromList("AudioManager").GetComponent<AudioManager>();
        _hoverSound = FMODUnity.RuntimeManager.CreateInstance(_eventPath);
    }

    // Update is called once per frame
    void Update()
    {
        if (isButtonHovering)
        {
            if (currentTimer <= 0)
            {
                if (currentSprite < spritesheet.Length)
                {
                    currentSprite++;
                    _image.sprite = spritesheet[currentSprite - 1];
                }
                currentTimer = timer;
            }
            else
            {
                //60 = second
                currentTimer-= Time.deltaTime * 60;
            }
        }
        else
        {
            currentSprite = 0;
            currentTimer = 0;
            _image.sprite = spritesheet[0];
        }
    }
}
