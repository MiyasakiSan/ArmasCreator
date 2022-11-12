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
        private UserDataManager userData;

        public Dictionary<string,PresetModel> SavePresetsDict;

        public void Init(UserDataManager userData)
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

        public void SavePreset(string mapId, QuestInfo questInfo)
        {
            bool exist = SavePresetsDict.TryGetValue(mapId, out PresetModel presetModel);

            if (exist)
            {
                if(SavePresetsDict[mapId].QuestInfoList.Exists(x => x.PresetId == questInfo.PresetId))
                {
                    var questIndex = SavePresetsDict[mapId].QuestInfoList.FindIndex(x => x.PresetId == questInfo.PresetId);
                    SavePresetsDict[mapId].QuestInfoList[questIndex] = questInfo;
                }
                else
                {
                    SavePresetsDict[mapId].QuestInfoList.Add(questInfo);
                }
            }
            else
            {
                var questInfoList = new List<QuestInfo>();
                var preset = new PresetModel();

                questInfoList.Add(questInfo);

                preset.QuestInfoList = questInfoList;

                SavePresetsDict.Add(mapId, preset);
            }

            SavePresetsDict[mapId].QuestInfoList.Sort(SortByPresetId);

            userData.UserData.UpdateSavePreset(mapId, questInfo);
        }

        public int GetIntFromPresetId(string presetId)
        {
            foreach(char i in presetId)
            {
                if(!int.TryParse(i.ToString(),out int result)) { continue; }

                if( result == 0) { continue; }

                return result;
            }

            return 0;
        }

        public int SortByPresetId(QuestInfo q1, QuestInfo q2)
        {
            return GetIntFromPresetId(q1.PresetId).CompareTo(GetIntFromPresetId(q2.PresetId));
        }
    }
}
    
