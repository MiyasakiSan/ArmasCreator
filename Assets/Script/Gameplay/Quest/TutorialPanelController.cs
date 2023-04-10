using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArmasCreator.UI;
using ArmasCreator.Utilities;
using ArmasCreator.Gameplay;

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

    private LoadingPopup loadingPopup = SharedContext.Instance.Get<LoadingPopup>();

    [SerializeField]
    private GameplayController gameplayController = SharedContext.Instance.Get<GameplayController>();

    void Start()
    {
        if(PlayerPrefs.GetInt("IsShowTutorial") == 1)
        {
            tutorialImage.gameObject.SetActive(false);
            backButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
            previousButton.gameObject.SetActive(false);
            gameplayController.OnTutorial.Invoke(false);
            Cursor.lockState = CursorLockMode.Locked;
            GameObject.FindWithTag("Player").GetComponent<PlayerRpgMovement>().canMove = true;
            return;
        }
        tutorialImage.sprite = tutorialSprite[currentPage];
        backButton.onClick.AddListener(() => 
        {
            tutorialImage.gameObject.SetActive(false);
            backButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
            previousButton.gameObject.SetActive(false);
            gameplayController.OnTutorial.Invoke(false);
            PlayerPrefs.SetInt("IsShowTutorial", 1);
            Cursor.lockState = CursorLockMode.Locked;
            GameObject.FindWithTag("Player").GetComponent<PlayerRpgMovement>().canMove = true;
        });
        nextButton.onClick.AddListener(() =>
        {
            OnToNextPage();
        });
        previousButton.onClick.AddListener(() =>
        {
            OnToPreviousPage();
        });

        StartCoroutine(waitForFadeBlack());
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
        //GameObject.FindWithTag("Player").GetComponent<PlayerRpgMovement>().canMove = false;
    }

    void Update()
    {

    }

    IEnumerator waitForFadeBlack()
    {
        yield return new WaitUntil(() => loadingPopup.IsFadingBlack);
        yield return new WaitForSeconds(1.8f);
        Cursor.lockState = CursorLockMode.Confined;
        GameObject.FindWithTag("Player").GetComponent<PlayerRpgMovement>().canMove = false;
        if (gameplayController != null)
        {
            gameplayController.OnTutorial.Invoke(true);
        }

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
