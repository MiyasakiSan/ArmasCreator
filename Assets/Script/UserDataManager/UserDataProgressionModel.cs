using ArmasCreator.LocalFile;
using UnityEngine;
using ArmasCreator.GameData;
using System.Collections.Generic;
using ArmasCreator.Utilities;
using ArmasCreator.UI;

namespace ArmasCreator.UserData
{
    public class UserDataProgressionModel
    {
        public Dictionary<string, int> Achievements;

        public Dictionary<QuestType, CurrentQuestModel> AllQuest = new Dictionary<QuestType, CurrentQuestModel>();

        private UserDataManager userDataManager;
        public void Init(UserDataManager userData)
        {
            this.userDataManager = userData;
        }

        public void SetupUserProgression(UserSaveDataModel saveModel)
        {
            Achievements = new Dictionary<string, int>();

            Achievements = saveModel.Achievements;

            AllQuest = saveModel.AllQuest;
        }

        public void UpdateAchievementProgression(string achievementId, int progress)
        {
            var gameDataManager = SharedContext.Instance.Get<GameDataManager>();

            bool exist = gameDataManager.TryGetAchievementInfo(achievementId, out var info);

            if (!exist)
            {
                Debug.LogError($"{nameof(UpdateAchievementProgression)} ====> achievement ID : {achievementId} doesn't exist in GameData");
                return;
            }

            exist = Achievements.TryGetValue(achievementId, out int value);

            if (!exist)
            {
                Debug.LogError($"{nameof(UpdateAchievementProgression)} ====> achievement ID : {achievementId} doesn't exist in UserData");
                return;
            }

            if (Achievements[achievementId] + progress > info.Progress)
            {
                Debug.LogError($"{nameof(UpdateAchievementProgression)} ====> achievement ID : {achievementId} already finish");
                return;
            }

            Achievements[achievementId] += progress;

            userDataManager.UserData.UpdateSaveAchievement(achievementId, Achievements[achievementId]);
        }

        public void ClaimAchievementReward(string achievementId)
        {
            var gameDataManager = SharedContext.Instance.Get<GameDataManager>();

            bool exist = gameDataManager.TryGetAchievementInfo(achievementId, out AchievementModel achievementInfo);

            if (!exist)
            {
                //TODO : Some Exception;
            }

            foreach (var reward in achievementInfo.Rewards)
            {
                var subType = gameDataManager.GetItemSubType(reward.Key);

                userDataManager.UserData.UserDataInventory.AddItem(reward.Key, subType, reward.Value);
            }
        }

        public void StartQuest(string questId)
        {
            var gameDataManager = SharedContext.Instance.Get<GameDataManager>();

            var questType = gameDataManager.GetQuestType(questId);

            var questSubType = gameDataManager.GetQuestSubType(questId);

            if (questType == QuestType.None)
            {
                Debug.LogError("Quest Type doesn't exist");
            }

            if (questSubType == QuestSubType.None)
            {
                Debug.LogError("Quest SubType doesn't exist");
            }

            if (questSubType == QuestSubType.Conversation)
            {
                var exist = gameDataManager.TryGetConversationQuestInfo(questId, out ConversationQuestModel questInfo);

                if (!exist)
                {
                    Debug.LogError("Quest doesn't exist");
                }

                AllQuest[questType].ConversationQuestIds.Add(questId);

                AllQuest[questType].LatestActiveQuest = questId;

                var dialogueManager = SharedContext.Instance.Get<DialogueManager>();

                dialogueManager.SetDialogueSequence(questInfo.StartDialogueId);
            }
        }

        public void EndQuest(string questId)
        {
            var gameDataManager = SharedContext.Instance.Get<GameDataManager>();

            var questType = gameDataManager.GetQuestType(questId);

            var questSubType = gameDataManager.GetQuestSubType(questId);

            if (questType == QuestType.None)
            {
                Debug.LogError("Quest Type doesn't exist");
            }

            if (questSubType == QuestSubType.None)
            {
                Debug.LogError("Quest SubType doesn't exist");
            }

            if (questSubType == QuestSubType.Conversation)
            {
                var exist = gameDataManager.TryGetConversationQuestInfo(questId, out ConversationQuestModel questInfo);

                if (!exist)
                {
                    Debug.LogError("Quest doesn't exist");
                }

                AllQuest[questType].ConversationQuestIds.Remove(questId);

                AllQuest[questType].FinishedQuestIds.Add(questId);

                AllQuest[questType].LatestActiveQuest = "";

                foreach(var rewards in questInfo.Rewards)
                {
                    userDataManager.UserData.UserDataInventory.AddItem(rewards.Key, SubType.None, rewards.Value);
                }
            }
        }
    }
}

