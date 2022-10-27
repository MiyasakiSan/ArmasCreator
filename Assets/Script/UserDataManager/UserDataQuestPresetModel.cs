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
    [Serializable]

    public class PresetModel
    {
        public List<QuestInfo> QuestInfoList = new List<QuestInfo>();
    }

    public class UserDataQuestPresetModel 
    {
        private UserDataModel userData;

        public Dictionary<string,PresetModel> SavePresetsDict;

        public void Init(UserDataModel userData)
        {
            this.userData = userData;
        }

        public void SetupUserQuestPrest(UserSaveDataModel saveModel)
        {
            SavePresetsDict = new Dictionary<string, PresetModel>();

            foreach(var savePreset in saveModel.AllSavePresets)
            {
                var presets = new PresetModel();

                presets.QuestInfoList = savePreset.Presets;

                SavePresetsDict.Add(savePreset.MapId, presets);
            }
        }

        public void GetAllSavePresetFromMapId(string mapId,out List<QuestInfo> questInfoList)
        {
            bool exist = SavePresetsDict.TryGetValue(mapId, out PresetModel presets);

            if (!exist) 
            {
                questInfoList = new List<QuestInfo>();
                return;
            }

            questInfoList = presets.QuestInfoList;
        }
    }
}
    
