using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ArmasCreator.Gameplay;

namespace ArmasCreator.UserData
{
    [Serializable]
    public class UserSaveDataModel 
    {
        public string UserId;

        public int Coins;

        public List<UserSavePresetModel> AllSavePresets = new List<UserSavePresetModel>();

        public Dictionary<string, int> ConsumableItems = new Dictionary<string, int>();

        public Dictionary<string, bool> EquipableItems = new Dictionary<string, bool>();
    }

    public class UserSavePresetModel
    {
        public string MapId;
        public List<QuestInfo> Presets = new List<QuestInfo>(); 
    }
}

