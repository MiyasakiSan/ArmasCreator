using ArmasCreator.GameData;
using ArmasCreator.Gameplay;
using ArmasCreator.UI;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuestAssistant : MonoBehaviour
{
    [SerializeField]
    private Outline QuestAssistantOutline;

    [SerializeField]
    private UIRotateTowardPlayer interactUI;

    [SerializeField]
    private GameObject questVcam;

    [SerializeField]
    private NPCQuestController nPCQuestController;

    private GameplayController gameplayController;
    private UserDataManager userDataManager;

    private void Awake()
    {
        gameplayController = SharedContext.Instance.Get<GameplayController>();
        userDataManager = SharedContext.Instance.Get<UserDataManager>();

        QuestAssistantOutline.OutlineWidth = 0f;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) { return; }

        QuestAssistantOutline.OutlineWidth = 4.5f;

        interactUI.Show();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) { return; }

        QuestAssistantOutline.OutlineWidth = 0f;

        interactUI.Hide();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) { return; }

        if (Input.GetKeyDown(KeyCode.T) && !questVcam.activeSelf)
        {
            ShowQuestAssistantCanvas();
            other.GetComponent<PlayerRpgMovement>().canMove = false;
            other.GetComponent<PlayerRpgMovement>().ResetAnimBoolean();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && questVcam.activeSelf)
        {
            HideQuestAssistantCanvas();
            other.GetComponent<PlayerRpgMovement>().canMove = true;
        }

        //Test Quest
        if (Input.GetKeyDown(KeyCode.G))
        {
            userDataManager.UserData.UserDataProgression.StartQuest("tr-00");
        }

    }

    public void ShowQuestAssistantCanvas()
    {
        gameplayController.Interacable = false;
        nPCQuestController.ShowQuestHeader();
    }

    public void HideQuestAssistantCanvas()
    {
        gameplayController.Interacable = true;
        nPCQuestController.HideQuestHeader();
    }
}
