using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanelController : MonoBehaviour
{
    [SerializeField]
    private Image tutorialImage;

    [SerializeField]
    private Sprite[] tutorialSprite;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Button nextButton;

    [SerializeField]
    private Button previousButton;

    private int currentPage = 0;

    void Start()
    {
        if(PlayerPrefs.GetInt("IsShowTutolial") == 1)
        {
            tutorialImage.gameObject.SetActive(false);
            backButton.gameObject.SetActive(false);
            return;
        }
        tutorialImage.sprite = tutorialSprite[currentPage];
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        backButton.onClick.AddListener(() => 
        {
            tutorialImage.gameObject.SetActive(false);
            backButton.gameObject.SetActive(false);
            PlayerPrefs.SetInt("IsShowTutolial", 1);
        });
        nextButton.onClick.AddListener(() =>
        {
            OnToNextPage();
        });
        previousButton.onClick.AddListener(() =>
        {
            OnToPreviousPage();
        });
    }

    public void OnToNextPage()
    {
        currentPage += 1;
        previousButton.gameObject.SetActive(true);
        if (currentPage > tutorialSprite.Length - 2)
        {
            nextButton.gameObject.SetActive(false);
            currentPage = tutorialSprite.Length - 1;
        }
        tutorialImage.sprite = tutorialSprite[currentPage];
    }

    public void OnToPreviousPage()
    {
        currentPage -= 1;
        nextButton.gameObject.SetActive(true);
        if (currentPage < 1)
        {
            previousButton.gameObject.SetActive(false);
            currentPage = 0;
        }
        tutorialImage.sprite = tutorialSprite[currentPage];
    }
}
