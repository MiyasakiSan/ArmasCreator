using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanelController : MonoBehaviour
{
    [SerializeField]
    private Image tutorialImage;

    [SerializeField]
    private Button backButton;

    void Start()
    {
        if(PlayerPrefs.GetInt("IsShowTutolial") == 1)
        {
            tutorialImage.gameObject.SetActive(false);
            backButton.gameObject.SetActive(false);
            return;
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        backButton.onClick.AddListener(() => 
        {
            tutorialImage.gameObject.SetActive(false);
            backButton.gameObject.SetActive(false);
            PlayerPrefs.SetInt("IsShowTutolial", 1);
        });
    }
}
