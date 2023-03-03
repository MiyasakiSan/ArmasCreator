using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmasCreator.UserData;
using ArmasCreator.GameData;
using ArmasCreator.Utilities;
using TMPro;


namespace ArmasCreator.UI
{
    public class ProfileQuestPanelController : MonoBehaviour
    {
        private UserDataManager userDataManager;

        private GameDataManager gameDataManager;

        private List<GameObject> sideQuestNodeList = new List<GameObject>();

        [SerializeField]
        private GameObject sideQuestNode;

        [SerializeField]
        private Transform sideQuestContent;

        [SerializeField]
        private TMP_Text mainQuestText;

        [SerializeField]
        private GameObject mainQuestDetailContent;

        private void Awake()
        {
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
        }

        public void SetUpQuestPanel()
        {
            SetUpMainQuest();
            PopulateSideQuestNode();
        }

        public void DeactivateQuestPanel()
        {
            if (sideQuestNodeList != null)
            {
                foreach (GameObject sideQuestNode in sideQuestNodeList)
                {
                    Destroy(sideQuestNode);
                }

                sideQuestNodeList.Clear();
            }
        }

        public void PopulateSideQuestNode()
        {
            var currentSideQuest = userDataManager.UserData.UserDataProgression.AllQuest[QuestType.Side];

            if (currentSideQuest.LatestActiveQuest == null)
            {
                Debug.Log("Don't have LatestActiveQuest");
                return;
            }

            var exist = gameDataManager.TryGetConversationQuestInfo(currentSideQuest.LatestActiveQuest, out var conversationQuestInfo);

            if (!exist)
            {
                Debug.Log($"Quest Id {currentSideQuest.LatestActiveQuest} not Exist");
                return;
            }

            var ins_SideQuestNode = Instantiate(sideQuestNode, sideQuestContent);
            sideQuestNodeList.Add(ins_SideQuestNode);
            ins_SideQuestNode.GetComponentInChildren<TMP_Text>().text = conversationQuestInfo.Name;
        }

        public void SetUpMainQuest()
        {
            var currentMainQuest = userDataManager.UserData.UserDataProgression.AllQuest[QuestType.Main];
            if (currentMainQuest.LatestActiveQuest == null)
            {
                mainQuestDetailContent.SetActive(false);
                Debug.Log("Don't have LatestActiveQuest");
                return;
            }

            var exist = gameDataManager.TryGetConversationQuestInfo(currentMainQuest.LatestActiveQuest, out var conversationQuestInfo);

            if (!exist)
            {
                mainQuestDetailContent.SetActive(false);
                Debug.Log($"Quest Id {currentMainQuest.LatestActiveQuest} not Exist");
                return;
            }

            mainQuestDetailContent.SetActive(true);
            mainQuestText.text = conversationQuestInfo.Name;
        }
    }
}
