using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ArmasCreator.Gameplay;
using ArmasCreator.GameData;

namespace ArmasCreator.UserData
{
    [Serializable]
    public class UserSaveDataModel 
    {
        public string UserId;

        public int Coins;

        public List<UserSavePresetModel> AllSavePresets = new List<UserSavePresetModel>();

        public Dictionary<QuestType, CurrentQuestModel> AllQuest = new Dictionary<QuestType, CurrentQuestModel>();

        public Dictionary<string, int> Achievements = new Dictionary<string, int>();

        public Dictionary<string, int> ConsumableItems = new Dictionary<string, int>();

        public Dictionary<SubType, EquipmentModel> EquipableItems = new Dictionary<SubType, EquipmentModel>();

        public Dictionary<string, int> CraftableItems = new Dictionary<string, int>();

        public List<string> recipes = new List<string>();
    }

    public class UserSavePresetModel
    {
        public string MapId;
        public List<QuestInfo> Presets = new List<QuestInfo>(); 
    }

    public class EquipmentModel
    {
        public string EquippedId;

        public List<string> UnlockIds = new List<string>();
    }

    public class CurrentQuestModel
    {
        public List<string> DestinationQuestIds = new List<string>();
        public List<string> EliminationQuestIds = new List<string>();
        public List<string> SurvivalQuestIds = new List<string>();
        public List<string> RequestQuestIds = new List<string>();
        public List<string> ConversationQuestIds = new List<string>();
        
        public List<string> FinishedQuestIds = new List<string>();

        public string LatestActiveQuest;
    }
}

