using ArmasCreator.GameData;
using ArmasCreator.UI;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHolder : MonoBehaviour
{
    [SerializeField]
    private string npcId;

    [SerializeField]
    public List<string> HolderQuestIds;

    [SerializeField]
    private Outline QuestHolderOutline;

    [SerializeField]
    private UIRotateTowardPlayer interactUI;

    [SerializeField]
    private GameObject mainQuestUI;

    [SerializeField]
    private GameObject sideQuestUI;

    private DialogueManager dialogueManager;
    private UserDataManager userDataManager;
    private GameDataManager gameDataManager;

    [SerializeField]
    private string latestMainQuestId;
    [SerializeField]
    private string latestSideQuestId;

    private QuestModel activeQuestInfo;

    bool onDialogue;

    void Start()
    {
        dialogueManager = SharedContext.Instance.Get<DialogueManager>();
        userDataManager = SharedContext.Instance.Get<UserDataManager>();
        gameDataManager = SharedContext.Instance.Get<GameDataManager>();

        CheckCurrentQuest();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) { return; }

        QuestHolderOutline.OutlineWidth = 4.5f;

        interactUI.Show();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) { return; }

        QuestHolderOutline.OutlineWidth = 0f;

        interactUI.Hide();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) { return; }

        if (onDialogue) { return; }

        if (!Input.GetKeyDown(KeyCode.T)) { return; }

        if (activeQuestInfo == null)
        {
            Debug.Log("Default dialogue");
            return;
        }

        dialogueManager.SetDialogueSequence(activeQuestInfo.EndDialogueId);
        dialogueManager.CurrentQuestInfo = activeQuestInfo;
        onDialogue = true;
    }

    public void CheckCurrentQuest()
    {
        activeQuestInfo = null;

        latestMainQuestId = userDataManager.UserData.UserDataProgression.AllQuest[ArmasCreator.GameData.QuestType.Main].LatestActiveQuest;
        latestSideQuestId = userDataManager.UserData.UserDataProgression.AllQuest[ArmasCreator.GameData.QuestType.Side].LatestActiveQuest;

        mainQuestUI.gameObject.SetActive(false);
        sideQuestUI.gameObject.SetActive(false);

        if (!string.IsNullOrEmpty(latestMainQuestId))
        {
            if (HolderQuestIds.Contains(latestMainQuestId))
            {
                gameDataManager.TryGetConversationQuestInfo(latestMainQuestId, out ConversationQuestModel questInfo);

                activeQuestInfo = questInfo;

                mainQuestUI.gameObject.SetActive(true);

                return;
            }
        }

        if (!string.IsNullOrEmpty(latestSideQuestId))
        {
            if (HolderQuestIds.Contains(latestSideQuestId))
            {
                gameDataManager.TryGetConversationQuestInfo(latestSideQuestId, out ConversationQuestModel questInfo);

                activeQuestInfo = questInfo;

                sideQuestUI.gameObject.SetActive(true);

                return;
            }
        }
    }
}
