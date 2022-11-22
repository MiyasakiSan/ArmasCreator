using ArmasCreator.LocalFile;
using UnityEngine;
using ArmasCreator.GameData;
using System.Collections.Generic;
using ArmasCreator.Utilities;

namespace ArmasCreator.UserData
{
    public class UserDataProgressionModel 
    {
        public Dictionary<string, int> Achievements;

        private UserDataManager userDataManager;
        public void Init(UserDataManager userData)
        {
            this.userDataManager = userData;
        }

        public void SetupUserProgression(UserSaveDataModel saveModel)
        {
            Achievements = new Dictionary<string, int>();

            Achievements = saveModel.Achievements;
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

            if(Achievements[achievementId] + progress > info.Progress)
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

            foreach(var reward in achievementInfo.Rewards)
            {
                var subType = gameDataManager.GetItemSubType(reward.Key);

                userDataManager.UserData.UserDataInventory.AddItem(reward.Key, subType, reward.Value);
            }
        }
    }
}

