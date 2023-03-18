using ArmasCreator.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    private SoundManager soundManager;

    [SerializeField]
    private ButtonType buttonType;
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (buttonType)
        {
            case ButtonType.Decision:
                {
                    break;
                }
            case ButtonType.Mainmenu:
                {
                    soundManager.PlayOneShot(soundManager.fModEvent.ButtonSFX, this.gameObject);
                    break;
                }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (buttonType)
        {
            case ButtonType.Decision:
                {
                    break;
                }
            case ButtonType.Mainmenu:
                {
                    soundManager.PlayOneShot(soundManager.fModEvent.MouseHoverSFX, this.gameObject);
                    Debug.Log("Hover");
                    break;
                }
        }
    }

    private void Awake()
    {
        soundManager = SharedContext.Instance.Get<SoundManager>();
    }

    private enum ButtonType
    {
        None,
        Mainmenu,
        Decision,
        Inventory
    }
}
