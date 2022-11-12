using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmasCreator.LocalFile;
using UnityEngine;
using ArmasCreator.Gameplay;
using ArmasCreator.GameData;
using ArmasCreator.Utilities;

namespace ArmasCreator.UserData
{
    public class UserDataModel 
    {
        private string userID;
        public string UserID => userID;

        private UserSaveDataModel saveModel;

        //private UserDataGameSettingModel userDataGameSetting;
        //public UserDataGameSettingModel UserDataGameSetting => userDataGameSetting;

        private UserDataProgressionModel userDataProgression;
        public UserDataProgressionModel UserDataProgression => userDataProgression;

        private UserDataQuestPresetModel userDataQuestPreset;
        public UserDataQuestPresetModel UserDataQuestPreset => userDataQuestPreset;

        private UserDataInventoryModel userDataInventory;
        public UserDataInventoryModel UserDataInventory => userDataInventory;

        public const string UserSaveDataLocal = "UserSaveDataLocal";

        public void Init(UserDataManager userDataManager)
        {
            //userDataGameSetting = new UserDataGameSettingModel();
            userDataProgression = new UserDataProgressionModel();
            userDataProgression.Init(userDataManager);

            userDataInventory = new UserDataInventoryModel();
            userDataInventory.Init(userDataManager);

            userDataQuestPreset = new UserDataQuestPresetModel();
            userDataQuestPreset.Init(userDataManager);

            LoadUserDataFromLocalData();
        }

        private void TestSaveUserData()
        {
            saveModel = new UserSaveDataModel();
            var testPreset = new List<UserSavePresetModel>();
            var testQuestInfo = new List<QuestInfo>();

            var firstSlot = new QuestInfo("Challenge1", "testMap", 1.5f, 1.0f, 0.5f, 3000);
            var secondSlot = new QuestInfo("Challenge2", "testMap", 0.5f, 1.0f, 1.0f, 3000);
            var thirdSlot = new QuestInfo("Challenge3", "testMap", 1.0f, 1.5f, 0.5f, 3000);

            testQuestInfo.Add(firstSlot);
            testQuestInfo.Add(secondSlot);
            testQuestInfo.Add(thirdSlot);

            testPreset.Add(new UserSavePresetModel
            {
                MapId = "Challenge1",
                Presets = testQuestInfo
            });

            saveModel.UserId = "tew";
            saveModel.Coins = 1000;
            saveModel.AllSavePresets = testPreset;

            SaveUserDataToLocal();
        }

        public void LoadUserDataFromLocalData()
        {
            var jsonString = LocalFileManager.LoadFromPersistentDataPath(UserSaveDataLocal);

            if (string.IsNullOrEmpty(jsonString))
            {
                return;
            }

            try
            {
                var data = JsonConvert.DeserializeObject<UserSaveDataModel>(jsonString);

                // prevent the case UserSaveDataModel can't be deserialized
                if (data == null) return;

                saveModel = data;

                SetupUserData();

            }
            catch (Exception ex)
            {
                Debug.LogError("UserDataModel: fail to LoadUserDataFromLocalData(), exception: " + ex.Message);
            }
        }

        private void SetupUserData()
        {
            Debug.Log("Setup UserData");

            userID = saveModel.UserId;
            PlayerPrefs.SetString("PName", userID);

            userDataQuestPreset.SetupUserQuestPrest(saveModel);
            userDataInventory.SetupUserInventory(saveModel);
        }

        public void SaveUserDataToLocal()
        {
            try
            {
                Debug.Log(Application.persistentDataPath);

                LocalFileManager.SaveToLocal(saveModel, Application.persistentDataPath, UserSaveDataLocal);
            }
            catch (Exception ex)
            {
                Debug.LogError("UserDataModel: fail to LoadUserDataFromLocalData(), exception: " + ex.Message);
            }
        }

        public void SetUserID(string name)
        {
            GameDataManager gameDataManager = SharedContext.Instance.Get<GameDataManager>();

            userID = name;
            saveModel = new UserSaveDataModel();
            saveModel.UserId = name;
            saveModel.AllSavePresets = new List<UserSavePresetModel>();
            saveModel.ConsumableItems = gameDataManager.GetAllInitConsumeItems();
            saveModel.EquipableItems = gameDataManager.GetAllInitEquipItems();
            saveModel.Achievements = gameDataManager.GetAllInitAchievements();
            userDataQuestPreset.SetupUserQuestPrest(saveModel);

            PlayerPrefs.SetString("PName", name);
        }

        public void UpdateSavePreset(string mapId, QuestInfo questInfo)
        {
            if(saveModel.AllSavePresets.Exists(x =>x.MapId == mapId))
            {
                var preset = saveModel.AllSavePresets.Find(x => x.MapId == mapId);

                if(preset.Presets.Exists(x =>x.PresetId == questInfo.PresetId))
                {
                    var questIndex = preset.Presets.FindIndex(x => x.PresetId == questInfo.PresetId);
                    preset.Presets[questIndex] = questInfo;
                }
                else
                {
                    preset.Presets.Add(questInfo);
                }
            }
            else
            {
                var questInfoList = new List<QuestInfo>();
                questInfoList.Add(questInfo);

                saveModel.AllSavePresets.Add(new UserSavePresetModel
                {
                    MapId = mapId,
                    Presets = questInfoList
                });
            }

            var savePreset = saveModel.AllSavePresets.Find(x => x.MapId == mapId);
            savePreset.Presets.Sort(userDataQuestPreset.SortByPresetId);

            SaveUserDataToLocal();
        }
    }
}

    
