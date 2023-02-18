using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmasCreator.GameData;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;
using UnityEngine.UI;
using TMPro;

namespace ArmasCreator.UI
{
    public class NPCQuestController : MonoBehaviour
    {
        [SerializeField]
        private Button mainQuestButton;

        [SerializeField]
        private Button subQuestButton;

        [SerializeField]
        private Transform questViewContent;

        [SerializeField]
        private NPCQuestNode npcQuestNode;

        [SerializeField]
        private NPCQuestDetailController npcQuestDetailController;

        [SerializeField]
        private Button acceptButton;

        [SerializeField]
        private Animator questPanelAnimator;

        private GameDataManager gameDataManager;

        private UserDataManager userDataManager;

        private List<NPCQuestNode> nPCQuestNodeList = new List<NPCQuestNode>();

        private void Awake()
        {
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
        }

        private void Start()
        {
            TopPanelButton mainQuestButton_TopPanelButton = mainQuestButton.GetComponent<TopPanelButton>();
            mainQuestButton.onClick.AddListener(() => 
            {
                DeselectedAllTopButton();
                mainQuestButton_TopPanelButton.OnSelected();
                ClearNPCQuestNode();
                PopulateNPCQuestNode(QuestType.Main);
                hideQuestDetail();
                ShowQuestView();
            });
            TopPanelButton subQuestButton_TopPanelButton = subQuestButton.GetComponent<TopPanelButton>();
            subQuestButton.onClick.AddListener(() =>
            {
                DeselectedAllTopButton();
                subQuestButton_TopPanelButton.OnSelected();
                ClearNPCQuestNode();
                PopulateNPCQuestNode(QuestType.Side);
                hideQuestDetail();
                ShowQuestView();
            });
        }

        public void DeselectedAllTopButton()
        {
            mainQuestButton.GetComponent<TopPanelButton>().OnDeSelected();
            subQuestButton.GetComponent<TopPanelButton>().OnDeSelected();
        }

        public void PopulateNPCQuestNode(QuestType questType)
        {
            var allConversationQuest = gameDataManager.GetAllConversationQuestInfo();

            List<ConversationQuestModel> conversationQuestList = new List<ConversationQuestModel>(SelectConversationQuestInfoByType(allConversationQuest, questType));

            foreach (ConversationQuestModel conversationQuestInfo in conversationQuestList)
            {
                var isCurrentQuestIsFinished = CheckQuestIsFinished(conversationQuestInfo, questType);

                var isAllPreviousQuestAreComplete = CheckAllPreviousQuestAreComplete(conversationQuestInfo, questType);

                if (!isCurrentQuestIsFinished)
                {
                    Debug.Log("Quest id {conversationQuestInfo.Id} is complete");
                    continue;
                }

                if (!isAllPreviousQuestAreComplete)
                {
                    Debug.Log("Quest id {conversationQuestInfo.Id} some previous quest not complete");
                    continue;
                }

                var ins_NPCQuesNode = Instantiate(npcQuestNode, questViewContent);
                ins_NPCQuesNode.SetDisplayData(conversationQuestInfo.Id);
                ins_NPCQuesNode.onClick.AddListener(() =>
                {
                    npcQuestDetailController.ClearRewardNodeList();
                    StartCoroutine(npcQuestDetailController.PopulateRewardNode(conversationQuestInfo));
                    ShowQuestDetail();
                    acceptButton.onClick.RemoveAllListeners();
                    acceptButton.onClick.AddListener(() =>
                    {
                        OnClickAcceptButton(conversationQuestInfo);
                    });
                });
                nPCQuestNodeList.Add(ins_NPCQuesNode);
            }
        }

        public void OnClickAcceptButton(ConversationQuestModel conversationQuestInfo)
        {
            //ToDo: Add Play Dialog or someting else that you want to do
        }

        public bool CheckAllPreviousQuestAreComplete(ConversationQuestModel conversationQuestInfo,QuestType questType)
        {
            bool AllPreviousQuestAreComplete = true;

            List<string> finishedQuestIds = new List<string>(userDataManager.UserData.UserDataProgression.AllQuest[questType].FinishedQuestIds);

            foreach (string previousQuestId in conversationQuestInfo.PreviousQuestId)
            {
                bool exist = finishedQuestIds.Contains(previousQuestId);

                if (!exist)
                {
                    AllPreviousQuestAreComplete = false;
                }
            }
            
            return AllPreviousQuestAreComplete;
        }

        public bool CheckQuestIsFinished(ConversationQuestModel conversationQuestInfo, QuestType questType)
        {
            var finishedQuestIds = userDataManager.UserData.UserDataProgression.AllQuest[questType].FinishedQuestIds;

            bool exist = finishedQuestIds.Contains(conversationQuestInfo.Id);

            return !exist;
        }

        public List<ConversationQuestModel> SelectConversationQuestInfoByType(Dictionary<string,ConversationQuestModel> allConversationQuest, QuestType questType)
        {
            List<ConversationQuestModel> conversationQuestList = new List<ConversationQuestModel>();

            foreach (ConversationQuestModel conversationQuestInfo in allConversationQuest.Values)
            {
                if (questType == conversationQuestInfo.Type)
                {
                    conversationQuestList.Add(conversationQuestInfo);
                }
            }

            return conversationQuestList;
        }

        public void ClearNPCQuestNode()
        {
            if(nPCQuestNodeList.Count == 0)
            {
                return;
            }

            foreach(NPCQuestNode nPCQuestNode in nPCQuestNodeList)
            {
                Destroy(nPCQuestNode.gameObject);
            }
            nPCQuestNodeList.Clear();
        }

        public void ShowQuestHeader()
        {
            questPanelAnimator.SetTrigger("showQuestHeader");
        }

        public void HideQuestHeader()
        {
            questPanelAnimator.SetTrigger("hideQuestDetail");
            questPanelAnimator.SetTrigger("hideQuestView");
            questPanelAnimator.SetTrigger("hideQuestHeader");
        }

        public void ShowQuestView()
        {
            questPanelAnimator.SetTrigger("showQuestView");
        }

        public void ShowQuestDetail()
        {
            questPanelAnimator.SetTrigger("showQuestDetail");
        }

        public void hideQuestView()
        {
            questPanelAnimator.SetTrigger("hideQuestView");
        }

        public void hideQuestDetail()
        {
            questPanelAnimator.SetTrigger("hideQuestDetail");
        }

        public void ResetTrigger(string triggerName)
        {
            questPanelAnimator.ResetTrigger(triggerName);
        }

        public void ResetAllTrigger()
        {
            questPanelAnimator.ResetTrigger("hideQuestDetail");
            questPanelAnimator.ResetTrigger("hideQuestView");
            questPanelAnimator.ResetTrigger("hideQuestHeader");
            questPanelAnimator.ResetTrigger("showQuestDetail");
            questPanelAnimator.ResetTrigger("showQuestView");
            questPanelAnimator.ResetTrigger("showQuestHeader");
        }
    }
}
