using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmasCreator.GameData;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;
using UnityEngine.UI;

namespace ArmasCreator.UI
{
    public class ChallengesPanelController : MonoBehaviour
    {
        [SerializeField]
        private ChallengeNode challengeNode;

        [SerializeField]
        private ChallengeDetailPanelController challengeDetailPanelController;

        [SerializeField]
        private ProfilePanelController profilePanelController;

        [SerializeField]
        private GameObject mainContent;

        [SerializeField]
        private Transform contentTranform;

        private GameDataManager gameDataManager;

        private UserDataManager userDataManager;

        private List<ChallengeNode> challengeNodes = new List<ChallengeNode>();

        [SerializeField]
        private GameObject challengeDetailContent;

        // Start is called before the first frame update
        void Awake()
        {
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
        }

        public void SetUpChallengePanel()
        {
            PopulateChallengesNode();
            SetUpAllChallengeNodeButton();
        }

        public void DeactiveChallengePanel()
        {
            if (challengeNodes != null)
            {
                foreach (ChallengeNode challengeNode in challengeNodes)
                {
                    Destroy(challengeNode.gameObject);
                }

                challengeNodes.Clear();
            }
        }

        private void SetUpAllChallengeNodeButton()
        {
            foreach (ChallengeNode challengeNode in challengeNodes)
            {
                challengeNode.SetUpButton();
            }
        }

        public void PopulateChallengesNode()
        {
            Dictionary<string,int> allAchievement = gameDataManager.GetAllInitAchievements();

            foreach (string achievementID in allAchievement.Keys)
            {
                ChallengeNode ins_ChallengeNode = Instantiate(challengeNode, contentTranform);
                challengeNodes.Add(ins_ChallengeNode);
                ins_ChallengeNode.SetChallengeDetailPanelController(challengeDetailPanelController);
                ins_ChallengeNode.SetChallengePanelController(this);
                ins_ChallengeNode.SetAchievementID(achievementID);
                ins_ChallengeNode.SetChallengeDetailContent(challengeDetailContent);
                var achievementExist = gameDataManager.TryGetAchievementInfo(achievementID, out var achievementInfo);
                if (!achievementExist)
                {
                    Debug.LogError("Fail to find info for a itemId: " + achievementID);
                    Destroy(ins_ChallengeNode.gameObject);

                    return;
                }
                ins_ChallengeNode.SetHeaderText(achievementInfo.Header + $"<color=#FF0000>{achievementInfo.Progress} </color>" + "Times");
                bool checkPass = achievementInfo.Progress <= userDataManager.UserData.UserDataProgression.Achievements[achievementInfo.ID];
                ins_ChallengeNode.SetCheckPass(checkPass);
            }
        }

        public void PlayChallengeDetailFadeIn()
        {
            profilePanelController.ActiveChallengeDetailAnim();
        }

        public void OnDeSelectedAll()
        {
            foreach(ChallengeNode challengeNode in challengeNodes)
            {
                challengeNode.OnDeSelectedChallengeNode();
            }
        }
    }
}
