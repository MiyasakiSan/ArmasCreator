using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmasCreator.LocalFile;
using UnityEngine;
using ArmasCreator.Gameplay;

namespace ArmasCreator.UserData
{
    public class UserDataModel 
    {
        private string userID;
        public string UserID => userID;

        private UserSaveDataModel saveModel;

        //private UserDataGameSettingModel userDataGameSetting;
        //public UserDataGameSettingModel UserDataGameSetting => userDataGameSetting;

        //private UserDataGameProgressionModel userDataGameProgression;
        //public UserDataGameProgressionModel UserDataGameProgression => userDataGameProgression;

        //private UserDataInventoryModel userDataInventory;
        //public UserDataInventoryModel UserDataInventory => userDataInventory;

        private UserDataQuestPresetModel userDataQuestPreset;
        public UserDataQuestPresetModel UserDataQuestPreset => userDataQuestPreset;


        public const string UserSaveDataLocal = "UserSaveDataLocal";

        public void Init(UserDataManager userDataManager)
        {
            //userDataGameSetting = new UserDataGameSettingModel();
            //userDataGameProgression = new UserDataGameProgressionModel();
            //userDataGameProgression.Init(userDataManager);

            //userDataInventory = new userDataInventoryModel();  

            userDataQuestPreset = new UserDataQuestPresetModel();
            userDataQuestPreset.Init(this);

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
            userID = name;
            saveModel = new UserSaveDataModel();
            saveModel.UserId = name;
            saveModel.AllSavePresets = new List<UserSavePresetModel>();

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

    