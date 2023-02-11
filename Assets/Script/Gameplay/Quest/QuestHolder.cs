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
    public List<string> HolderQuestIds;

    [SerializeField]
    private Outline QuestHolderOutline;

    [SerializeField]
    private UIRotateTowardPlayer interactUI;

    private DialogueManager dialogueManager;
    private UserDataManager userDataManager;
    private GameDataManager gameDataManager;

    private string latestMainQuestId;
    private string latestSideQuestId;

    private QuestModel activeQuestInfo;

    bool onDialogue;

    void Start()
    {
        dialogueManager = SharedContext.Instance.Get<DialogueManager>();
        userDataManager = SharedContext.Instance.Get<UserDataManager>();

        latestMainQuestId = userDataManager.UserData.UserDataProgression.AllQuest[ArmasCreator.GameData.QuestType.Main].LatestActiveQuest;
        latestSideQuestId = userDataManager.UserData.UserDataProgression.AllQuest[ArmasCreator.GameData.QuestType.Side].LatestActiveQuest;

        if (HolderQuestIds.Contains(latestMainQuestId))
        {
            gameDataManager.TryGetConversationQuestInfo(latestMainQuestId, out ConversationQuestModel questInfo);

            activeQuestInfo = questInfo;

            return;
        }

        if (HolderQuestIds.Contains(latestSideQuestId))
        {
            gameDataManager.TryGetConversationQuestInfo(latestSideQuestId, out ConversationQuestModel questInfo);

            activeQuestInfo = questInfo;

            return;
        }
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
            dialogueManager.SetDialogueSequence("Default Dialogue");
            return;
        }

        dialogueManager.SetDialogueSequence(activeQuestInfo.NpcDialogueId);
        dialogueManager.CurrentQuestInfo = activeQuestInfo;
        onDialogue = true;

    }
}
